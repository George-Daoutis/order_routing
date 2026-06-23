using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace order_routing.Server.Migrations
{
    /// <inheritdoc />
    public partial class CurrentMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "StoreId", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$11$hdK6GmVpIihFMTAwigXL0Ov4Q5fuE3uayVlHgbEeQn7ZNkEjCDrfy", "Admin", null, "admin" },
                    { 2, "$2a$11$4vkFvLlv66iIotzFskanX.PNIYozDrGmxRYpS9Cg/EQnI0PfOIUYq", "StoreUser", 1, "store1" }
                });
        }
    }
}
