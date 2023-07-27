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
				ISBN = Guid.NewGuid().ToString()[..12].ToUpper(),
				ListPrice = 100,
				PriceFifty = 70,
				PriceHundredOrMore = 60,
				CategoryId = 1,
				ImageUrl = ""
			},
			new Product
			{
				Id = 2,
				Title = "Alamut",
				Author = "Vladimir Bartol",
				ISBN = Guid.NewGuid().ToString()[..12].ToUpper(),
				ListPrice = 150,
				PriceFifty = 80,
				PriceHundredOrMore = 70,
				CategoryId = 2,
				ImageUrl = ""
			},
			new Product
			{
				Id = 3,
				Title = "Fortune of Time",
				Author = "Billy Spark",
				ISBN = "SWD9999001",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ListPrice = 99,
				PriceFifty = 85,
				PriceHundredOrMore = 80,
				CategoryId = 2
			},
			new Product
			{
				Id = 4,
				Title = "Dark Skies",
				Author = "Nancy Hoover",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ISBN = "CAW777777701",
				ListPrice = 40,
				PriceFifty = 25,
				PriceHundredOrMore = 20,
				CategoryId = 2
			},
			new Product
			{
				Id = 5,
				Title = "Vanish in the Sunset",
				Author = "Julian Button",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ISBN = "RITO5555501",
				ListPrice = 55,
				PriceFifty = 40,
				PriceHundredOrMore = 35,
				CategoryId = 2
			},
			new Product
			{
				Id = 6,
				Title = "Cotton Candy",
				Author = "Abby Muscles",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ISBN = "WS3333333301",
				ListPrice = 70,
				PriceFifty = 60,
				PriceHundredOrMore = 55,
				CategoryId = 2
			},
			new Product
			{
				Id = 7,
				Title = "Rock in the Ocean",
				Author = "Ron Parker",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ISBN = "SOTJ1111111101",
				ListPrice = 30,
				PriceFifty = 25,
				PriceHundredOrMore = 20,
				CategoryId = 2
			},
			new Product
			{
				Id = 8,
				Title = "Leaves and Wonders",
				Author = "Laura Phantom",
				Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
				ISBN = "FOT000000001",
				ListPrice = 25,
				PriceFifty = 22,
				PriceHundredOrMore = 20,
				CategoryId = 2
			}
		);
	}
}
