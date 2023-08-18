using Bulky.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration;
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        var categories = new List<Category>()
        {
            new() { Id = 1, Name = "Action", DisplayOrder = 1 },
            new() { Id = 2, Name = "Sci-Fi", DisplayOrder = 2 },
            new() { Id = 3, Name = "History", DisplayOrder = 3 }
        };
        builder.HasData(categories);
    }
}
