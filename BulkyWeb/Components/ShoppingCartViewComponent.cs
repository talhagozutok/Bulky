using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Components;

public class ShoppingCartViewComponent : ViewComponent
{
    private readonly IUnitOfWork _unitOfWork;

    public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var session = HttpContext.Session;
        var sessionCart = StaticDetails.SessionCart;

        if (userId is not null)
        {
            if (session.GetInt32(sessionCart) is null)
            {
                session.SetInt32(sessionCart, _unitOfWork.ShoppingCarts
                .GetAll(u => u.ApplicationUserId.Equals(userId))!.Count());
            }

            return View(session.GetInt32(sessionCart));
        }
        else
        {
            session.Clear();
            return View(0);
        }
    }
}
