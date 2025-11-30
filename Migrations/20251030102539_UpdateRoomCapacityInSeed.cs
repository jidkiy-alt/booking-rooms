using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNET_PROJECT.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomCapacityInSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Capacity",
                value: 9);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Capacity",
                value: 8);
        }
    }
}
