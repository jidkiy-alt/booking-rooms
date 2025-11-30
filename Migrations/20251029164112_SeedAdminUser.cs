using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNET_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "FirstName", "LastName", "CreatedAt", "RoleId" },
                values: new object[] { 1, "admin@example.com", "admin123", "Администратор", "Системы", DateTime.Now, 2 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
