using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var companyList = _unitOfWork.Companies.GetAll();
        return View(companyList);
    }

    public IActionResult Upsert([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return View(new Company());
        }

        var company = _unitOfWork.Companies.Get(c => c.Id.Equals(id));

        return company is not null ? View(company) : NotFound();
    }

    [HttpPost]
    public IActionResult Upsert(Company company)
    {
        if (string.Equals(company.Name, "test", StringComparison.OrdinalIgnoreCase))
        {
            MemberInfo? property = typeof(Company).GetProperty(nameof(Company.Name));
            var dd = property?.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            ModelState.AddModelError(nameof(Company.Name), $"'Test' is an invalid value for {dd?.Name}.");
        }

        if (ModelState.IsValid)
        {
            if (company.Id == 0)
            {
                _unitOfWork.Companies.Add(company);
                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";

                return RedirectToAction("Index");
            }
            else
            {
                _unitOfWork.Companies.Update(company);
                _unitOfWork.Save();
                TempData["success"] = "Company updated successfully";

                return RedirectToAction("Index");
            }
        }

        return View(company);
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute(Name = "id")] int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        var company = _unitOfWork.Companies.Get(c => c.Id.Equals(id));

        if (company is not null)
        {
            _unitOfWork.Companies.Remove(company);
            _unitOfWork.Save();

			return RedirectToAction("Index");
		}

		return NotFound();
    }

    #region API

    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList = _unitOfWork.Companies.GetAll();
        return Json(new { data = companyList});
    }

	#endregion API

}
