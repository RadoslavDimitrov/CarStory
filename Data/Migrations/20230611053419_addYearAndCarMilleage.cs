using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStory.Data.Migrations
{
    public partial class addYearAndCarMilleage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Milleage",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "YearManufactured",
                table: "Cars",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Milleage",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "YearManufactured",
                table: "Cars");
        }
    }
}
