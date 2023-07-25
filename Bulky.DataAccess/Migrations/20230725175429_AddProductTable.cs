using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    PriceFifty = table.Column<double>(type: "float", nullable: false),
                    PriceHundredOrMore = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Sci-Fi");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "Description", "ISBN", "ListPrice", "PriceFifty", "PriceHundredOrMore", "Title" },
                values: new object[,]
                {
                    { 1, "Stephen King", "", "df2c504c-c0c4-41c0-80ee-501c41a63f8b", 100.0, 70.0, 60.0, "22/11/63" },
                    { 2, "Vladimir Bartol", "", "1dac59c1-fb67-43a9-a0cd-7fb8ff7de7fb", 100.0, 70.0, 60.0, "Alamut" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "SciFi");
        }
    }
}
