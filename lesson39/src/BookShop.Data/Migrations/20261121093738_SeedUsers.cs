using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShop.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "UserName", "Role", "PasswordHash" },
                values: new object[,]
                {
                    { 1, "alice", "Manager", BCrypt.Net.BCrypt.HashPassword("secret") },
                    { 2, "bob", "Staff", BCrypt.Net.BCrypt.HashPassword("secret") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValues: new object[] { 1, 2 });
        }
    }
}
