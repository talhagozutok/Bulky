using System.Net;
using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Product = Bulky.Models.Entities.Product;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area(areaName: "Admin")]
[Authorize]
public class OrderController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    [BindProperty]
    public OrderViewModel OrderViewModel { get; set; }

    public OrderController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(int orderId)
    {
        OrderViewModel = new()
        {
            OrderHeader = _unitOfWork.OrderHeaders.Get(o => o.Id.Equals(orderId), includeProperties: nameof(ApplicationUser))!,
            OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderHeaderId.Equals(orderId), includeProperties: nameof(Product))
        };

        return View(OrderViewModel);
    }

    [HttpPost]
    [Authorize(Roles = $"{StaticDetails.Role_Admin}, {StaticDetails.Role_Employee}")]
    public IActionResult UpdateOrderDetail()
    {
        var orderHeaderFromDb = _unitOfWork.OrderHeaders
            .Get(o => o.Id.Equals(OrderViewModel.OrderHeader.Id));

        if (orderHeaderFromDb is not null)
        {
            orderHeaderFromDb.Name = OrderViewModel.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderViewModel.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderViewModel.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderViewModel.OrderHeader.City;
            orderHeaderFromDb.State = OrderViewModel.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderViewModel.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.Carrier))
            {
                orderHeaderFromDb.Carrier = OrderViewModel.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderViewModel.OrderHeader.TrackingNumber))
            {
                orderHeaderFromDb.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
            }

            _unitOfWork.OrderHeaders.Update(orderHeaderFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Order details updated successfully.";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id }); ;
        }

        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = $"{StaticDetails.Role_Admin}, {StaticDetails.Role_Employee}")]
    public IActionResult StartProcessing()
    {
        _unitOfWork.OrderHeaders.UpdateStatus(
            id: OrderViewModel.OrderHeader.Id,
            orderStatus: StaticDetails.StatusInProcess);

        _unitOfWork.Save();
        TempData["success"] = "Order details updated successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id }); ;
    }


    [HttpPost]
    [Authorize(Roles = $"{StaticDetails.Role_Admin}, {StaticDetails.Role_Employee}")]
    public IActionResult ShipOrder()
    {
        var orderHeader = _unitOfWork.OrderHeaders.Get(o => o.Id.Equals(OrderViewModel.OrderHeader.Id));

        orderHeader.TrackingNumber = OrderViewModel.OrderHeader.TrackingNumber;
        orderHeader.Carrier = OrderViewModel.OrderHeader.Carrier;
        orderHeader.OrderStatus = StaticDetails.StatusShipped;
        orderHeader.ShippingDate = DateTime.Now;

        if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
        {
            orderHeader.PaymentDueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30));
        }

        _unitOfWork.OrderHeaders.Update(orderHeader);
        _unitOfWork.Save();

        TempData["success"] = "Order shipped successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
    }


    [HttpPost]
    [Authorize(Roles = $"{StaticDetails.Role_Admin}, {StaticDetails.Role_Employee}")]
    public IActionResult CancelOrder()
    {
        var orderHeader = _unitOfWork.OrderHeaders.Get(o => o.Id.Equals(OrderViewModel.OrderHeader.Id));

        if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusApproved)
        {
            var options = new RefundCreateOptions
            {
                Reason = RefundReasons.RequestedByCustomer,
                PaymentIntent = orderHeader.PaymentIntentId
            };

            var service = new RefundService();
            Refund refund = service.Create(options);

            _unitOfWork.OrderHeaders.UpdateStatus(orderHeader.Id,
                StaticDetails.StatusCancelled,
                StaticDetails.StatusRefunded);
        }
        else
        {
            _unitOfWork.OrderHeaders.UpdateStatus(orderHeader.Id,
                StaticDetails.StatusCancelled,
                StaticDetails.StatusCancelled);
        }

        _unitOfWork.Save();
        TempData["success"] = "Order cancelled successfully.";
        return RedirectToAction(nameof(Details), new { orderId = OrderViewModel.OrderHeader.Id });
    }

    [ActionName("Details")]
    [HttpPost]
    public IActionResult DetailsPayNow()
    {
        OrderViewModel.OrderHeader = _unitOfWork.OrderHeaders
            .Get(o => o.Id.Equals(OrderViewModel.OrderHeader.Id), includeProperties: nameof(ApplicationUser))!;
        OrderViewModel.OrderDetails = _unitOfWork.OrderDetails
            .GetAll(o => o.OrderHeaderId.Equals(OrderViewModel.OrderHeader.Id), includeProperties: nameof(Product));

        // stripe logic
        var origin = Request.Scheme + "://" + Request.Host.Value;
        var options = new SessionCreateOptions
        {
            SuccessUrl = origin + $"/Admin/Order/PaymentConfirmation?orderHeaderId={OrderViewModel.OrderHeader.Id}",
            CancelUrl = origin + $"/Admin/Order/Details?orderId={OrderViewModel.OrderHeader.Id}",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in OrderViewModel.OrderDetails)
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
        _unitOfWork.OrderHeaders.UpdateStripePaymentID(OrderViewModel.OrderHeader.Id, session.Id, session.PaymentIntentId);
        _unitOfWork.Save();
        Response.Headers.Add("Location", session.Url);

        return new StatusCodeResult((int)HttpStatusCode.RedirectMethod);
    }

    public IActionResult PaymentConfirmation(int orderHeaderId)
    {
        OrderHeader orderHeader = _unitOfWork.OrderHeaders.Get(u => u.Id == orderHeaderId);

        if (orderHeader.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
        {
            // this is an order by company
            // because only company users pay net30 
            // therefore if `PaymentStatus` is equals to `PaymentStatusDelayedPayment`
            // then this clause only applies for company users.

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);

            if (session.PaymentStatus.Equals("paid", StringComparison.OrdinalIgnoreCase))
            {
                _unitOfWork.OrderHeaders.UpdateStripePaymentID(orderHeaderId, session.Id, session.PaymentIntentId);
                _unitOfWork.OrderHeaders.UpdateStatus(orderHeaderId, orderHeader.OrderStatus!, StaticDetails.PaymentStatusApproved);
                _unitOfWork.Save();
            }
        }

        return View(orderHeaderId);
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var order = _unitOfWork.OrderHeaders.Get(p => p.Id.Equals(id));

        if (order is not null)
        {
            _unitOfWork.OrderHeaders.Remove(order);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        return NotFound();
    }

    #region API

    [HttpGet]
    public IActionResult GetAll(string status)
    {
        IEnumerable<OrderHeader> orderHeaderList;

        if (User.IsInRole(StaticDetails.Role_Admin) || User.IsInRole(StaticDetails.Role_Employee))
        {
            orderHeaderList = _unitOfWork.OrderHeaders
                .GetAll(includeProperties: nameof(ApplicationUser));
        }
        else
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            orderHeaderList = _unitOfWork.OrderHeaders
                .GetAll(o => o.ApplicationUserId.Equals(userId), includeProperties: nameof(ApplicationUser));
        }

        orderHeaderList = status switch
        {
            "paymentPending" => orderHeaderList.Where(o => o.PaymentStatus.Equals(StaticDetails.PaymentStatusPending)),
            "inProcess" => orderHeaderList.Where(o => o.OrderStatus.Equals(StaticDetails.StatusInProcess)),
            "completed" => orderHeaderList.Where(o => o.OrderStatus.Equals(StaticDetails.StatusShipped)),
            "approved" => orderHeaderList.Where(o => o.OrderStatus.Equals(StaticDetails.StatusApproved)),
            _ => orderHeaderList
        };

        return Json(new { data = orderHeaderList });
    }

    #endregion
}
