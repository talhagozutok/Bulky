using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utilities;
using Stripe;
using Bulky.DataAccess.Initializers;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.
services.AddControllersWithViews();
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
services.AddAuthentication().AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = configuration.GetValue<string>("Facebook:AppId");
    facebookOptions.AppSecret = configuration.GetValue<string>("Facebook:AppSecret");
});

services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

services.AddDistributedMemoryCache();
services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.AddRazorPages();

services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICompanyRepository, CompanyRepository>();
services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
services.AddScoped<IEmailSender, EmailSender>();
services.AddScoped<IDbInitializer, DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

StripeConfiguration.ApiKey = configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

SeedDatabase();
app.UseSession();

app.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapAreaControllerRoute(
        name: "Customer",
        areaName: "Customer",
        pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}"
    );

app.MapRazorPages();

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}