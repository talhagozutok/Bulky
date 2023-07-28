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
	private readonly IWebHostEnvironment _webHostEnvironment;

	public ProductController(IUnitOfWork unitOfWork,
		IWebHostEnvironment webHostEnvironment)
	{
		_unitOfWork = unitOfWork;
		_webHostEnvironment = webHostEnvironment;
	}

	public IActionResult Index()
	{
		var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: nameof(Category)).ToList();
		return View(productList);
	}

	// [UP]date and in[SERT] functionality.
	public IActionResult Upsert([FromRoute(Name = "id")] int? id)
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
			string wwwRootPath = _webHostEnvironment.WebRootPath;

			if (file is not null)
			{
				string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
				string productImagesPath = Path.Combine(wwwRootPath, @"images\product");

				if (!string.IsNullOrEmpty(viewModel.Product.ImageUrl))
				{
					// Delete the old image
					var oldImagePath =
						Path.Combine(wwwRootPath, viewModel.Product.ImageUrl.TrimStart('\\'));

					if (System.IO.File.Exists(oldImagePath))
					{
						if (!viewModel.Product.ImageUrl.Contains("initial"))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
				}

				using (var fileStream = new FileStream(Path.Combine(productImagesPath, fileName), FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				viewModel.Product.ImageUrl = @"\images\product\" + fileName;
			}

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

	[HttpDelete]
	public IActionResult Delete([FromRoute(Name = "id")] int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var product = _unitOfWork.ProductRepository.Get(p => p.Id.Equals(id));

		if (product is not null)
		{
			if (product.ImageUrl is not null && !product.ImageUrl.Contains("initial"))
			{
			var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath,
				product.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
			{
					System.IO.File.Delete(oldImagePath);
				}
			}

			_unitOfWork.ProductRepository.Remove(product);
			_unitOfWork.Save();

			return RedirectToAction("Index");
		}

		return NotFound();
	}

	#region API

	[HttpGet]
	public IActionResult GetAll()
	{
		var productList = _unitOfWork.ProductRepository.GetAll(includeProperties: nameof(Category)).ToList();
		return Json(new { data = productList });
	}

	#endregion
}
