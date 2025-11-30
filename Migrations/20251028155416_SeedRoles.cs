using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNET_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name", "Description" },
                values: new object[,]
                {
                    { 1, "User", "Обычный пользователь" },
                    { 2, "Admin", "Администратор" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
