using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area(areaName: "Customer")]
// Only authorized users are able to access
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
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
                .GetAll(u => u.ApplicationUserId.Equals(userId), includeProperties: nameof(Product))
        };

        foreach (var cart in ShoppingCartViewModel.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartViewModel.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartViewModel);
    }

    public IActionResult Summary()
    {
        return View();
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
