using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace order_routing.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddedFulfillmentVariables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstVerification",
                table: "OrderLineFullfillments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SecondVerification",
                table: "OrderLineFullfillments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "fulfillmentTransportMethod",
                table: "OrderLineFullfillments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstVerification",
                table: "OrderLineFullfillments");

            migrationBuilder.DropColumn(
                name: "SecondVerification",
                table: "OrderLineFullfillments");

            migrationBuilder.DropColumn(
                name: "fulfillmentTransportMethod",
                table: "OrderLineFullfillments");
        }
    }
}
