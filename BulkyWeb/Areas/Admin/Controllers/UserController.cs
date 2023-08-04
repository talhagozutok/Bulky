using Bulky.DataAccess.Data;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class UserController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    public UserController(ApplicationDbContext db)
    {
        _dbContext = db;
    }
    public IActionResult Index()
    {
        return View();
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> userList = _dbContext.ApplicationUsers.Include(u => u.Company).ToList();

        return Json(new { data = userList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        return Json(new { success = true, message = "Deleted successfully" });
    }

    #endregion
}
