using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        _logger.LogWarning("Navigated to /Home/Index");

        IEnumerable<Product> productList = _unitOfWork.Products.GetAll(includeProperties: nameof(Category));
        return View(productList);
    }

    public IActionResult Details([FromQuery(Name = "productId")] int productId)
    {
        ShoppingCart cart = new()
        {
            Product = _unitOfWork.Products.Get(p => p.Id.Equals(productId), includeProperties: nameof(Category)),
            Count = 1,
            ProductId = productId
        };

        if (cart.Product is not null)
        {
            return View(cart);
        }

        return NotFound();
    }

    // Only authorized users are able to access
    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart cart)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        cart.ApplicationUserId = userId;

        ShoppingCart cartFromDatabase = _unitOfWork.ShoppingCarts
            .Get(u => u.ApplicationUserId.Equals(userId) && u.ProductId.Equals(cart.ProductId))!;

        if (cartFromDatabase is not null)
        {
            // Shopping cart exists
            cartFromDatabase.Count += cart.Count;
            _unitOfWork.ShoppingCarts.Update(cartFromDatabase);
            _unitOfWork.Save();
        }
        else
        {
            // Add cart record
            _unitOfWork.ShoppingCarts.Add(cart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(StaticDetails.SessionCart, _unitOfWork.ShoppingCarts
                .GetAll(u => u.ApplicationUserId.Equals(userId))!.Count());
        }

        var product = _unitOfWork.Products.Get(p => p.Id.Equals(cart.ProductId))!;
        TempData["shoppingCartAdded"] = $"{product.Title} added to cart";

        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}