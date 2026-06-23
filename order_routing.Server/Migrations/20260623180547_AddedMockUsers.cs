using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace order_routing.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedMockUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "$2a$11$hdK6GmVpIihFMTAwigXL0Ov4Q5fuE3uayVlHgbEeQn7ZNkEjCDrfy", "admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "$2a$11$4vkFvLlv66iIotzFskanX.PNIYozDrGmxRYpS9Cg/EQnI0PfOIUYq", "store1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "admin", "Admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PasswordHash", "Username" },
                values: new object[] { "sUser1", "sUser1" });
        }
    }
}
