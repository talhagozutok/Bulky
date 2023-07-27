using System.Reflection.Metadata;
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

	// [UP]date and in[SERT] functionality.
	public IActionResult Upsert([FromRoute(Name = "id")]int? id)
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

		if (id is null || id == 0)
		{
			// Create
			return View(viewModel);
		}

		// Update
		Product? product = _unitOfWork.ProductRepository.Get(p => p.Id.Equals(id));
		if (product is not null)
		{
			viewModel.Product = product;
			return View(viewModel);
		}

		return NotFound();
	}

	[HttpPost]
	public IActionResult Upsert(ProductViewModel viewModel, IFormFile? file)
	{
		if (ModelState.IsValid)
		{
			if (viewModel.Product.Id == 0)
			{
				_unitOfWork.ProductRepository.Add(viewModel.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
			}
			else
			{
				_unitOfWork.ProductRepository.Update(viewModel.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product updated successfully";
			}

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
