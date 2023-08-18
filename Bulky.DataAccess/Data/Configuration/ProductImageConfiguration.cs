using Bulky.Models.Entities;
using Bulky.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration;
public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        var productImages = new List<ProductImage>()
        {
            new () { Id = 1, ProductId = 1, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\22-11-63.jpg" },
            new () { Id = 2, ProductId = 1, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\22-11-63 (1).jpg" },

            new () { Id = 3, ProductId = 2, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\alamut.jpg" },
            new () { Id = 4, ProductId = 2, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\alamut (1).jpg" },

            new () { Id = 5, ProductId = 3, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\fortune of time.jpg" },
            new () { Id = 6, ProductId = 3, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\fortune of time (1).jpg" },

            new () { Id = 7, ProductId = 4, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\dark skies.jpg" },
            new () { Id = 8, ProductId = 4, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\dark skies (1).jpg" },

            new () { Id = 9, ProductId = 5, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\vanish in the sunset.jpg" },
            new () { Id = 10, ProductId = 5, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\vanish in the sunset (1).jpg" },

            new () { Id = 11, ProductId = 6, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\cotton candy.jpg" },
            new () { Id = 12, ProductId = 6, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\cotton candy (1).jpg" },

            new () { Id = 13, ProductId = 7, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\rock in the ocean.jpg" },
            new () { Id = 14, ProductId = 7, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\rock in the ocean (1).jpg" },

            new () { Id = 15, ProductId = 8, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\leaves and wonders.jpg" },
            new () { Id = 16, ProductId = 8, ImageUrl = $"{StaticDetails.InitialImagePath}\\products\\leaves and wonders (1).jpg" }
        };
        builder.HasData(productImages);
    }
}
