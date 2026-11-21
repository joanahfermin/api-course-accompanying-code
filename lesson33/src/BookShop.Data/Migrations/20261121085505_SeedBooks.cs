using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Title", "Author", "Price", "Stock", "Category" },
                values: new object[,]
                {
                    { 1, "The Pragmatic Programmer", "Andrew Hunt", 39.90m, 12, "Programming" },
                    { 2, "Clean Code", "Robert C. Martin", 34.50m, 7, "Programming" },
                    { 3, "Refactoring", "Martin Fowler", 44.00m, 3, "Programming" },
                    { 4, "Salt Fat Acid Heat", "Samin Nosrat", 29.00m, 4, "Cooking" },
                    { 5, "The Joy of Cooking", "Irma Rombauer", 32.50m, 2, "Cooking" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2, 3, 4, 5 });
        }
    }
}
