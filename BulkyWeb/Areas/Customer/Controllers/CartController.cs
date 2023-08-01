using System.Net;
using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area(areaName: "Customer")]
// Only authorized users are able to access
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        ShoppingCartViewModel = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId.Equals(userId),
                             includeProperties: nameof(Product)),
            OrderHeader = new()
        };

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartViewModel);
    }

    public IActionResult Summary()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        ShoppingCartViewModel = new()
        {
            ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId.Equals(userId),
                             includeProperties: nameof(Product)),
            OrderHeader = new()
        };

        ShoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id.Equals(userId))!;

        ShoppingCartViewModel.OrderHeader.Name = ShoppingCartViewModel.OrderHeader.ApplicationUser.Name;
        ShoppingCartViewModel.OrderHeader.PhoneNumber = ShoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
        ShoppingCartViewModel.OrderHeader.StreetAddress = ShoppingCartViewModel.OrderHeader.ApplicationUser.StreetAddress;
        ShoppingCartViewModel.OrderHeader.City = ShoppingCartViewModel.OrderHeader.ApplicationUser.City;
        ShoppingCartViewModel.OrderHeader.State = ShoppingCartViewModel.OrderHeader.ApplicationUser.State;
        ShoppingCartViewModel.OrderHeader.PostalCode = ShoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartViewModel);
    }

    [HttpPost]
    [ActionName("Summary")]
    public IActionResult SummaryPost()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        ShoppingCartViewModel.ShoppingCartList = _unitOfWork.ShoppingCartRepository
                .GetAll(u => u.ApplicationUserId.Equals(userId), includeProperties: nameof(Product));

        ShoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;
        ShoppingCartViewModel.OrderHeader.ApplicationUserId = userId;
        
        ApplicationUser applicationUser = _unitOfWork.ApplicationUserRepository.Get(u => u.Id.Equals(userId))!;

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartViewModel.OrderHeader.OrderTotal += (cart.Price * cart.Count);
        }

        if (applicationUser
                .CompanyId
                .GetValueOrDefault() == 0)
        {
            // regular customer
            ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
            ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusPending;
        }
        else
        {
            // company user
            ShoppingCartViewModel.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
            ShoppingCartViewModel.OrderHeader.OrderStatus = StaticDetails.StatusApproved;
        }

        _unitOfWork.OrderHeaderRepository.Add(ShoppingCartViewModel.OrderHeader);
        _unitOfWork.Save();

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            OrderDetail orderDetail = new()
            {
                ProductId = cart.ProductId,
                OrderHeaderId = ShoppingCartViewModel.OrderHeader.Id,
                Price = cart.Price,
                Count = cart.Count
            };

            _unitOfWork.OrderDetailRepository.Add(orderDetail);
            _unitOfWork.Save();
        }

        if (applicationUser
                .CompanyId
                .GetValueOrDefault() == 0)
        {
			// regular customer account
            // stripe logic

			var origin = Request.Scheme + "://" + Request.Host.Value;
			var options = new SessionCreateOptions
			{
				SuccessUrl = origin + $"/Customer/Cart/OrderConfirmation?id={ShoppingCartViewModel.OrderHeader.Id}",
				CancelUrl = origin + "/Customer/Cart/Index",
				LineItems = new List<SessionLineItemOptions>(),
				Mode = "payment",
			};

			foreach (var item in ShoppingCartViewModel.ShoppingCartList)
			{
				var sessionLineItem = new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(item.Price * 100), // $20.50 => 2050
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.Product.Title
						}
					},
					Quantity = item.Count
				};
				options.LineItems.Add(sessionLineItem);
			}

			var service = new SessionService();
			Session session = service.Create(options);
			_unitOfWork.OrderHeaderRepository.UpdateStripePaymentID(ShoppingCartViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
			_unitOfWork.Save();
			Response.Headers.Add("Location", session.Url);
			return new StatusCodeResult((int)HttpStatusCode.RedirectMethod);
        }

		return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartViewModel.OrderHeader.Id });
    }

    public IActionResult OrderConfirmation(int id)
    {
		OrderHeader orderHeader = _unitOfWork.OrderHeaderRepository.Get(u => u.Id == id, includeProperties: "ApplicationUser");

		if (orderHeader.PaymentStatus != StaticDetails.PaymentStatusDelayedPayment)
		{
			// this is an order by customer
			// because only company users pay net30 
			// therefore if `PaymentStatus` is not equals to `PaymentStatusDelayedPayment`
			// then this clause only applies for other than company users.

			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
				_unitOfWork.OrderHeaderRepository.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
				_unitOfWork.OrderHeaderRepository.UpdateStatus(id, StaticDetails.StatusApproved, StaticDetails.PaymentStatusApproved);
				_unitOfWork.Save();
			}
		}

		List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCartRepository
			.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

		_unitOfWork.ShoppingCartRepository.RemoveRange(shoppingCarts);
		_unitOfWork.Save();

        return View(id);
    }

    public IActionResult Plus(int cartId)
    {
        var cartFromDatabase = _unitOfWork.ShoppingCartRepository.Get(u => u.Id.Equals(cartId));
        cartFromDatabase.Count += 1;
        _unitOfWork.ShoppingCartRepository.Update(cartFromDatabase);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cartFromDatabase = _unitOfWork.ShoppingCartRepository.Get(u => u.Id.Equals(cartId));

        if (cartFromDatabase.Count <= 1)
        {
            // remove that from cart
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDatabase);
        }
        else
        {
            cartFromDatabase.Count -= 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDatabase);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cartFromDatabase = _unitOfWork.ShoppingCartRepository.Get(u => u.Id.Equals(cartId));
        _unitOfWork.ShoppingCartRepository.Remove(cartFromDatabase);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if (shoppingCart.Count <= 50)
        {
            return shoppingCart.Product.Price;
        }
        else
        {
            if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.PriceFifty;
            }
            else
            {
                return shoppingCart.Product.PriceHundredOrMore;
            }
        }
    }
}
