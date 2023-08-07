using Bulky.DataAccess.Repository.Contracts;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

[Area(areaName: "Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class UserController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<IdentityUser> _userManager;

    public UserController(IUnitOfWork unitOfWork,
        RoleManager<IdentityRole> roleManager,
        UserManager<IdentityUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region API CALLS

    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> userList = _unitOfWork.ApplicationUsers
            .GetAll(includeProperties: nameof(Company))
            .ToList();

        foreach (var user in userList)
        {
            var userRoles = _userManager
                .GetRolesAsync(user)
                .GetAwaiter().GetResult()
                .ToList();

            user.Roles.AddRange(userRoles);
        }

        return Json(new { data = userList });
    }

    [HttpPost]
    public IActionResult LockUnlock([FromBody] string id)
    {
        var user = _userManager
            .FindByIdAsync(id)
            .GetAwaiter().GetResult();

        if (user is null)
        {
            return Json(new { success = false, message = "Error while locking/unlocking." });
        }

        if (user.LockoutEnd is not null && user.LockoutEnd > DateTime.Now)
        {
            // User is currently locked
            // we need to unlock them

            user.LockoutEnd = DateTime.Now;
            _unitOfWork.Save();

            return Json(new { success = true, message = "User unlocked successfully" });
        }
        else
        {
            // User is already unlocked
            // we need to lock them

            user.LockoutEnd = DateTime.Now.AddYears(1000);
            _unitOfWork.Save();

            return Json(new { success = true, message = "User locked successfully" });
        }
    }

    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        return Json(new { success = true, message = "User deleted successfully" });
    }

    #endregion
}
