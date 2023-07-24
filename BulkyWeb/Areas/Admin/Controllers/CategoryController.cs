using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Admin")]
public class CategoryController : Controller
{
	private readonly IUnitOfWork _unitOfWork;

	public CategoryController(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public IActionResult Index()
	{
		var categoryList = _unitOfWork.CategoryRepository.GetAll();
		return View(categoryList);
	}

	public IActionResult Create()
	{
		return View(new Category());
	}

	[HttpPost]
	public IActionResult Create(Category category)
	{
		if (string.Equals(category.Name, "test", StringComparison.OrdinalIgnoreCase))
		{
			MemberInfo? property = typeof(Category).GetProperty(nameof(Category.Name));
			var dd = property?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
			ModelState.AddModelError(nameof(Category.Name), $"'Test' is an invalid value for {dd?.Name}.");
		}

		if (ModelState.IsValid)
		{
			_unitOfWork.CategoryRepository.Add(category);
			_unitOfWork.Save();
			TempData["success"] = "Category created successfully";

			return RedirectToAction("Index");
		}

		return View();
	}

	public IActionResult Edit(int? id)
	{
		var category = _unitOfWork.CategoryRepository.Get(c => c.Id.Equals(id));
		return category is not null ? View(category) : NotFound();
	}

	[HttpPost]
	public IActionResult Edit(Category category)
	{
		if (ModelState.IsValid)
		{
			_unitOfWork.CategoryRepository.Update(category);
			_unitOfWork.Save();
			TempData["success"] = "Category updated successfully";

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

		var category = _unitOfWork.CategoryRepository.Get(c => c.Id.Equals(id));

		if (category is not null)
		{
			_unitOfWork.CategoryRepository.Remove(category);
			_unitOfWork.Save();
			TempData["delete"] = "Category deleted successfully";
			TempData["deleteText"] = $"Category name: {category.Name}";

			return RedirectToAction("Index");
		}

		return NotFound();
	}
}
