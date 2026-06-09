using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace order_routing.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedTestProductAndStoreData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ProductCode" },
                values: new object[,]
                {
                    { 1, "Item 1", "ITEM001" },
                    { 2, "Item 2", "ITEM002" },
                    { 3, "Item 3", "ITEM003" }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Address", "PhoneNumber", "StoreDescription" },
                values: new object[,]
                {
                    { 1, "Address 1", "2101234567", "Store 1" },
                    { 2, "Address 2", "2104501929", "Store 2" },
                    { 3, "Address 3", "2106316478", "Store 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stores",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
