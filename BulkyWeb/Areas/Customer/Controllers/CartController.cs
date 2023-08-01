using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            // it is a regular customer account and we need to capture payment
            // stripe logic
        }

        return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartViewModel.OrderHeader.Id});
    }

    public IActionResult OrderConfirmation(int id)
    {
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
