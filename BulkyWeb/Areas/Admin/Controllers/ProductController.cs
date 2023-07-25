using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area(areaName: "Admin")]
public class ProductController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public ProductController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		var productList = _unitOfWork.ProductRepository.GetAll().ToList();
		return View(productList);
	}

	public IActionResult Create()
	{
		return View(new Product());
	}

	[HttpPost]
	public IActionResult Create(Product product)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.ProductRepository.Add(product);
			_unitOfWork.Save();
			TempData["success"] = "Product created successfully";

			return RedirectToAction("Index");
		}

		return View();
	}

	public IActionResult Edit([FromRoute(Name = "id")] int? id)
	{
		var product = _unitOfWork.ProductRepository.Get(p => p.Id.Equals(id));
		return product is not null ? View(product) : NotFound();
	}

	[HttpPost]
	public IActionResult Edit(Product product)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.ProductRepository.Update(product);
			_unitOfWork.Save();
			TempData["success"] = "Product updated successfully";

			return RedirectToAction("Index");
		}

		return View();
	}

	[HttpGet]
	public IActionResult Delete([FromRoute(Name = "id")] int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var product = _unitOfWork.ProductRepository.Get(p => p.Id.Equals(id));

		if (product is not null)
		{
			_unitOfWork.ProductRepository.Remove(product);
			_unitOfWork.Save();
			TempData["delete"] = "Product deleted successfully.";
			TempData["deleteText"] = $"Product title: {product.Title}";

			return RedirectToAction("Index");
		}

		return NotFound();
	}
}
