using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bulky.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(550)", maxLength: 550, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ListPrice = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    PriceFifty = table.Column<double>(type: "float", nullable: false),
                    PriceHundredOrMore = table.Column<double>(type: "float", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Action" },
                    { 2, 2, "Sci-Fi" },
                    { 3, 3, "History" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "ImageUrl", "ListPrice", "Price", "PriceFifty", "PriceHundredOrMore", "Title" },
                values: new object[,]
                {
                    { 1, "Stephen King", 1, "11/22/63 is a novel by Stephen King about a time traveller who attempts to prevent the assassination of United States President John F. Kennedy, which occurred on November 22, 1963.", "06A2B06C-2CC", "\\images\\product\\initial\\22-11-63.jpg", 100.0, 80.0, 70.0, 60.0, "22/11/63" },
                    { 2, "Vladimir Bartol", 2, "Alamut is a novel by Vladimir Bartol, first published in 1938 in Slovenian, dealing with the story of Hassan-i Sabbah and the Hashshashin, and named after their Alamut fortress. The maxim of the novel is \"Nothing is an absolute reality; all is permitted\". This book was one of the inspirations for the video game series Assassin's Creed.", "BF16868D-B8D", "\\images\\product\\initial\\alamut.jpg", 150.0, 100.0, 80.0, 70.0, "Alamut" },
                    { 3, "Billy Spark", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SWD9999001", "\\images\\product\\initial\\fortune of time.jpg", 99.0, 90.0, 85.0, 80.0, "Fortune of Time" },
                    { 4, "Nancy Hoover", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "CAW777777701", "\\images\\product\\initial\\dark skies.jpg", 40.0, 30.0, 25.0, 20.0, "Dark Skies" },
                    { 5, "Julian Button", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "RITO5555501", "\\images\\product\\initial\\vanish in the sunset.jpg", 55.0, 50.0, 40.0, 35.0, "Vanish in the Sunset" },
                    { 6, "Abby Muscles", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "WS3333333301", "\\images\\product\\initial\\cotton candy.jpg", 70.0, 65.0, 60.0, 55.0, "Cotton Candy" },
                    { 7, "Ron Parker", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "SOTJ1111111101", "\\images\\product\\initial\\rock in the ocean.jpg", 30.0, 27.0, 25.0, 20.0, "Rock in the Ocean" },
                    { 8, "Laura Phantom", 2, "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ", "FOT000000001", "\\images\\product\\initial\\leaves and wonders.jpg", 25.0, 23.0, 22.0, 20.0, "Leaves and Wonders" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
