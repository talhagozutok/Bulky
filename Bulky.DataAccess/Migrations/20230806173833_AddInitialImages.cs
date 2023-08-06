using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 1, "\\images\\initial\\products\\22-11-63.jpg", 1 },
                    { 2, "\\images\\initial\\products\\22-11-63 (1).jpg", 1 },
                    { 3, "\\images\\initial\\products\\alamut.jpg", 2 },
                    { 4, "\\images\\initial\\products\\alamut (1).jpg", 2 },
                    { 5, "\\images\\initial\\products\\fortune of time.jpg", 3 },
                    { 6, "\\images\\initial\\products\\fortune of time (1).jpg", 3 },
                    { 7, "\\images\\initial\\products\\dark skies.jpg", 4 },
                    { 8, "\\images\\initial\\products\\dark skies (1).jpg", 4 },
                    { 9, "\\images\\initial\\products\\vanish in the sunset.jpg", 5 },
                    { 10, "\\images\\initial\\products\\vanish in the sunset (1).jpg", 5 },
                    { 11, "\\images\\initial\\products\\cotton candy.jpg", 6 },
                    { 12, "\\images\\initial\\products\\cotton candy (1).jpg", 6 },
                    { 13, "\\images\\initial\\products\\rock in the ocean.jpg", 7 },
                    { 14, "\\images\\initial\\products\\rock in the ocean (1).jpg", 7 },
                    { 15, "\\images\\initial\\products\\leaves and wonders.jpg", 8 },
                    { 16, "\\images\\initial\\products\\leaves and wonders (1).jpg", 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 16);
        }
    }
}
