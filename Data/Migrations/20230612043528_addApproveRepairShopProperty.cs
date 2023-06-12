using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarStory.Data.Migrations
{
    public partial class addApproveRepairShopProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CarRepairShops",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CarRepairShops");
        }
    }
}
