using Bulky.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration;
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasMany(p => p.ProductImages)
            .WithOne(p => p.Product)
            .HasForeignKey(p => p.ProductId)
            .IsRequired(false);

        builder.HasMany<ShoppingCart>()
            .WithOne(p => p.Product)
            .HasForeignKey(s => s.ProductId)
            .IsRequired(false);

        builder.HasMany<OrderDetail>()
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId)
            .IsRequired(true);

        var products = new List<Product>() {
            new ()
            {
                Id = 1,
                Title = "22/11/63",
                Author = "Stephen King",
                Description = "11/22/63 is a novel by Stephen King about a time traveller who attempts to prevent the assassination of United States President John F. Kennedy, which occurred on November 22, 1963.",
                ISBN = "74BCFEA0-209",
                ListPrice = 100,
                Price = 80,
                PriceFifty = 70,
                PriceHundredOrMore = 60,
                CategoryId = 1
            },
            new ()
            {
                Id = 2,
                Title = "Alamut",
                Author = "Vladimir Bartol",
                Description = "Alamut is a novel by Vladimir Bartol, first published in 1938 in Slovenian, dealing with the story of Hassan-i Sabbah and the Hashshashin, and named after their Alamut fortress. The maxim of the novel is \"Nothing is an absolute reality; all is permitted\". This book was one of the inspirations for the video game series Assassin's Creed.",
                ISBN = "C35256A1-420",
                ListPrice = 150,
                Price = 100,
                PriceFifty = 80,
                PriceHundredOrMore = 70,
                CategoryId = 2
            },
            new ()
            {
                Id = 3,
                Title = "Fortune of Time",
                Author = "Billy Spark",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SWD9999001",
                ListPrice = 99,
                Price = 90,
                PriceFifty = 85,
                PriceHundredOrMore = 80,
                CategoryId = 2
            },
            new ()
            {
                Id = 4,
                Title = "Dark Skies",
                Author = "Nancy Hoover",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "CAW777777701",
                ListPrice = 40,
                Price = 30,
                PriceFifty = 25,
                PriceHundredOrMore = 20,
                CategoryId = 2
            },
            new ()
            {
                Id = 5,
                Title = "Vanish in the Sunset",
                Author = "Julian Button",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "RITO5555501",
                ListPrice = 55,
                Price = 50,
                PriceFifty = 40,
                PriceHundredOrMore = 35,
                CategoryId = 2
            },
            new ()
            {
                Id = 6,
                Title = "Cotton Candy",
                Author = "Abby Muscles",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "WS3333333301",
                ListPrice = 70,
                Price = 65,
                PriceFifty = 60,
                PriceHundredOrMore = 55,
                CategoryId = 2
            },
            new ()
            {
                Id = 7,
                Title = "Rock in the Ocean",
                Author = "Ron Parker",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "SOTJ1111111101",
                ListPrice = 30,
                Price = 27,
                PriceFifty = 25,
                PriceHundredOrMore = 20,
                CategoryId = 2
            },
            new ()
            {
                Id = 8,
                Title = "Leaves and Wonders",
                Author = "Laura Phantom",
                Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                ISBN = "FOT000000001",
                ListPrice = 25,
                Price = 23,
                PriceFifty = 22,
                PriceHundredOrMore = 20,
                CategoryId = 2
            }

        };
        builder.HasData(products);
    }
}
