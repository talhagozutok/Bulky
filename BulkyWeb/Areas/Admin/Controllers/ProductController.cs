using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Models.ViewModels;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace BulkyWeb.Areas.Admin.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class ProductController : Controller
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IWebHostEnvironment _webHostEnvironment;

	public ProductController(IUnitOfWork unitOfWork,
		IWebHostEnvironment webHostEnvironment)
	{
		_unitOfWork = unitOfWork;
		_webHostEnvironment = webHostEnvironment;
	}

	public IActionResult Index()
	{
		var productList = _unitOfWork.Products.GetAll(includeProperties: nameof(Category)).ToList();
		return View(productList);
	}

	// [UP]date and in[SERT] functionality.
	public IActionResult Upsert([FromRoute(Name = "id")] int? id)
	{
		ProductViewModel viewModel = new()
		{
			Product = new Product(),
			CategoryList = _unitOfWork.Categories
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
		Product? product = _unitOfWork.Products.Get(p => p.Id.Equals(id));
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
			string wwwRootPath = _webHostEnvironment.WebRootPath;

			if (viewModel.Product.Id == 0)
			{
				_unitOfWork.Products.Add(viewModel.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
			}
			else
			{
				_unitOfWork.Products.Update(viewModel.Product);
				_unitOfWork.Save();
				TempData["success"] = "Product updated successfully";
            }

            return RedirectToAction("Index");
		}
		else
		{
			viewModel.CategoryList = _unitOfWork.Categories
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
		var product = _unitOfWork.Products.Get(p => p.Id.Equals(id));
		return product is not null ? View(product) : NotFound();
	}

	[HttpDelete]
	public IActionResult Delete([FromRoute(Name = "id")] int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var product = _unitOfWork.Products.Get(p => p.Id.Equals(id));

		if (product is not null)
		{
			_unitOfWork.Products.Remove(product);
			_unitOfWork.Save();

			return RedirectToAction("Index");
		}

		return NotFound();
	}

	#region API

	[HttpGet]
	public IActionResult GetAll()
	{
		var productList = _unitOfWork.Products.GetAll(includeProperties: nameof(Category)).ToList();
		return Json(new { data = productList });
	}

	#endregion
}
