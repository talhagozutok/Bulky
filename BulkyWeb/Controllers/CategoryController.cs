using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
	private readonly ApplicationDbContext _dbContext;
	public CategoryController(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	public IActionResult Index()
	{
		List<Category> objCategoryList = _dbContext.Categories.ToList();
		return View(objCategoryList);
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
			_dbContext.Categories.Add(category);
			_dbContext.SaveChanges();
			TempData["success"] = "Category created successfully";

			return RedirectToAction("Index");
		}

		return View();
	}

	public IActionResult Edit(int? id)
	{
		var category = _dbContext.Categories.Find(id);
		return category is not null ? View(category) : NotFound();
	}

	[HttpPost]
	public IActionResult Edit(Category category)
	{
		if (ModelState.IsValid)
		{
			_dbContext.Categories.Update(category);
			_dbContext.SaveChanges();
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

		var category = _dbContext.Categories.Find(id);

		if (category is not null)
		{
			_dbContext.Categories.Remove(category);
			_dbContext.SaveChanges();
			TempData["delete"] = "Category deleted successfully";
			TempData["deleteText"] = $"Category name: {category.Name}";

			return RedirectToAction("Index");
		}

		return NotFound();
	}
}
