using Bulky.DataAccess.Data;
using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Initializers;
public class DbInitializer : IDbInitializer
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _dbContext;

    public DbInitializer(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public void Initialize()
    {
        // Push migrations if they are not applied.
        try
        {
            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }
        }
        catch
        {
            throw;
        }

        // Create roles if they are not created.
        if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();

            // then we will create admin user as well.
            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = StaticDetails.AdminEmail,
                Email = StaticDetails.AdminEmail,
                Name = "Admin",
                PhoneNumber = "1234567890",
                StreetAddress = "Boss St.",
                State = "NY",
                PostalCode = "12345",
                City = "Buffalo"
            }, password: StaticDetails.AdminPassword).GetAwaiter().GetResult();

            ApplicationUser? user = _dbContext.ApplicationUsers.FirstOrDefault(
                u => u.Email! == StaticDetails.AdminEmail);
            _userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
