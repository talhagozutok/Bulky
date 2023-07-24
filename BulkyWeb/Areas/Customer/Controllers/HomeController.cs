using System.Diagnostics;
using Bulky.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Customer")]
public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;

	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	public IActionResult Index()
	{
		_logger.LogWarning("Navigated to /Home/Index");
		return View();
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