using Bulky.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data;

public class ApplicationDbContext : DbContext
{
	public DbSet<Category> Categories { get; set; }

	public DbSet<Product> Products { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Category>().HasData(
			new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
			new Category { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
			new Category { Id = 3, Name = "History", DisplayOrder = 3 }
			);

		modelBuilder.Entity<Product>().HasData(
			new Product
			{
				Id = 1,
				Title = "22/11/63",
				Author = "Stephen King",
				ISBN = Guid.NewGuid().ToString(),
				ListPrice = 100,
				PriceFifty = 70,
				PriceHundredOrMore = 60,
				CategoryId = 1
			}
			,
			new Product
			{
				Id = 2,
				Title = "Alamut",
				Author = "Vladimir Bartol",
				ISBN = Guid.NewGuid().ToString(),
				ListPrice = 150,
				PriceFifty = 80,
				PriceHundredOrMore = 70,
				CategoryId = 2
			}
		);
	}
}
