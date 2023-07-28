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
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ISBN", "ImageUrl" },
                values: new object[] { "11/22/63 is a novel by Stephen King about a time traveller who attempts to prevent the assassination of United States President John F. Kennedy, which occurred on November 22, 1963.", "1119FCB5-A4E", "\\images\\product\\initial\\22-11-63.jpg" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ISBN", "ImageUrl" },
                values: new object[] { "Alamut is a novel by Vladimir Bartol, first published in 1938 in Slovenian, dealing with the story of Hassan-i Sabbah and the Hashshashin, and named after their Alamut fortress. The maxim of the novel is \"Nothing is an absolute reality; all is permitted\". This book was one of the inspirations for the video game series Assassin's Creed.", "9F964FBB-AAD", "\\images\\product\\initial\\alamut.jpg" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "PriceFifty", "PriceHundredOrMore", "Title" },
                values: new object[,]
                {
                    { 3, "Billy Spark", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SWD9999001", "\\images\\product\\initial\\fortune of time.jpg", 99.0, 85.0, 80.0, "Fortune of Time" },
                    { 4, "Nancy Hoover", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "CAW777777701", "\\images\\product\\initial\\dark skies.jpg", 40.0, 25.0, 20.0, "Dark Skies" },
                    { 5, "Julian Button", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "RITO5555501", "\\images\\product\\initial\\vanish in the sunset.jpg", 55.0, 40.0, 35.0, "Vanish in the Sunset" },
                    { 6, "Abby Muscles", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WS3333333301", "\\images\\product\\initial\\cotton candy.jpg", 70.0, 60.0, 55.0, "Cotton Candy" },
                    { 7, "Ron Parker", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SOTJ1111111101", "\\images\\product\\initial\\rock in the ocean.jpg", 30.0, 25.0, 20.0, "Rock in the Ocean" },
                    { 8, "Laura Phantom", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "FOT000000001", "\\images\\product\\initial\\leaves and wonders.jpg", 25.0, 22.0, 20.0, "Leaves and Wonders" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "ISBN", "ImageUrl" },
                values: new object[] { "", "989992e2-48d2-40cb-b685-3fc765b65c73", "" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "ISBN", "ImageUrl" },
                values: new object[] { "", "596db6de-73c8-48e7-8589-b61cf44cb629", "" });
        }
    }
}
