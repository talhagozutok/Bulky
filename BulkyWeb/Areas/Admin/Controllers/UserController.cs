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

    [HttpPost]
    public IActionResult LockUnlock([FromBody] string id)
    {
        var user = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return Json(new { success = false, message = "Error while locking/unlocking." });
        }

        if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
        {
            // User is currently locked
            // we need to unlock them

            user.LockoutEnd = DateTime.Now;
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "User is locked" });
        }
        else
        {
            // User is already unlock
            // we need to lock them

            user.LockoutEnd = DateTime.Now.AddYears(1000);
            _dbContext.SaveChanges();

            return Json(new { success = true, message = "User is unlocked" });
        }
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        return Json(new { success = true, message = "Deleted successfully" });
    }

    #endregion
}
