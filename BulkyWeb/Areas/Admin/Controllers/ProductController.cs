using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
		ProductViewModel viewModel = new()
		{
			Product = new Product(),
			CategoryList = _unitOfWork.CategoryRepository
				.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString(),
				})
		};

		return View(viewModel);
	}

	[HttpPost]
	public IActionResult Create(ProductViewModel viewModel)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.ProductRepository.Add(viewModel.Product);
			_unitOfWork.Save();
			TempData["success"] = "Product created successfully";

			return RedirectToAction("Index");
		}
		else
		{
			viewModel.CategoryList = _unitOfWork.CategoryRepository
				.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString(),
				});

			return View(viewModel);
		}
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
