using Bulky.DataAccess.Data;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        List<IdentityRole> roleList = _dbContext.Roles.ToList();
        List<IdentityUserRole<string>> userRoles = _dbContext.UserRoles.ToList();

        foreach (var user in userList)
        {
            var userId = user.Id;
            var roleIds = userRoles.Where(r => r.UserId == userId);
            foreach (var roleId in roleIds)
            {
                var role = roleList.Find(r => r.Id == roleId.RoleId);
                user.Roles.Add(role.Name);
            }
        }

        return Json(new { data = userList });
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        return Json(new { success = true, message = "Deleted successfully" });
    }

    #endregion
}
