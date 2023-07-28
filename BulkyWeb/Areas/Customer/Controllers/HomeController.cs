using System.Diagnostics;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
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

		IEnumerable<Product> productList = _unitOfWork.ProductRepository.GetAll(includeProperties: nameof(Category));
		return View(productList);
	}

	public IActionResult Details([FromRoute(Name = "id")]int? id)
	{
		Product? product = _unitOfWork.ProductRepository.Get(p => p.Id.Equals(id), includeProperties: nameof(Category)); ;

		if (product is not null)
		{
			return View(product);
		}

		return NotFound();
	}

	public IActionResult Privacy()
	{
		return View();
	}
	public IActionResult Test()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
}