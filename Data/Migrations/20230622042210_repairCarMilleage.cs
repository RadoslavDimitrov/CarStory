using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStory.Data.Migrations
{
    public partial class repairCarMilleage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currCarMilleage",
                table: "Repairs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currCarMilleage",
                table: "Repairs");
        }
    }
}
