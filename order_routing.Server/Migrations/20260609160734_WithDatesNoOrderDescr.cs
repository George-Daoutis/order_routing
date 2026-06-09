using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace order_routing.Server.Migrations
{
    /// <inheritdoc />
    public partial class WithDatesNoOrderDescr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderLines");

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreationDate",
                table: "OrderLines",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "CreationDate",
                table: "OrderLineFullfillments",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "OrderLines");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "OrderLineFullfillments");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderLines",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
