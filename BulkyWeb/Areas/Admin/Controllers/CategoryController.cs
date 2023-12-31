﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var categoryList = _unitOfWork.Categories.GetAll();
        return View(categoryList);
    }

    public IActionResult Upsert([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return View(new Category());
        }

        var category = _unitOfWork.Categories.Get(c => c.Id.Equals(id));

        return category is not null ? View(category) : NotFound();
    }

    [HttpPost]
    public IActionResult Upsert(Category category)
    {
        if (string.Equals(category.Name, "test", StringComparison.OrdinalIgnoreCase))
        {
            MemberInfo? property = typeof(Category).GetProperty(nameof(Category.Name));
            var dd = property?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

            ModelState.AddModelError(
                nameof(Category.Name),
                $"'Test' is an invalid value for {dd?.Name}.");
        }

        if (ModelState.IsValid)
        {
            if (category.Id == 0)
            {
                _unitOfWork.Categories.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";

                return RedirectToAction(nameof(Index));
            }
            else
            {
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";

                return RedirectToAction(nameof(Index));
            }
        }

        return View(category);
    }

    [HttpGet]
    public IActionResult Delete([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var category = _unitOfWork.Categories.Get(c => c.Id.Equals(id));

        if (category is not null)
        {
            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Save();
            TempData["delete"] = "Category deleted successfully";
            TempData["deleteText"] = $"Category name: {category.Name}";

            return RedirectToAction(nameof(Index));
        }

        return NotFound();
    }
}
