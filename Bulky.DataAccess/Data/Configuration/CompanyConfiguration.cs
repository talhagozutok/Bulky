using Bulky.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bulky.DataAccess.Data.Configuration;
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        var companies = new List<Company>()
        {
            new ()
            {
                Id = 1,
                Name = "Tech Solution",
                City = "Tech City",
                State = "IL",
                StreetAddress = "123 Tech St",
                PostalCode = "12121",
                PhoneNumber = "6669990000"
            },
            new ()
            {
                Id = 2,
                Name = "Vivid Books",
                City = "Tech City",
                State = "IL",
                StreetAddress = "999 Vid St",
                PostalCode = "3333",
                PhoneNumber = "7779990000"
            },
            new ()
            {
                Id = 3,
                Name = "Readers Club",
                City = "Lala land",
                State = "NY",
                StreetAddress = "999 Main St",
                PostalCode = "89999",
                PhoneNumber = "1113335555"
            }
        };
        builder.HasData(companies);
    }
}
