using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballWorldWeb.Migrations
{
    public partial class AddOrderNoForGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Groups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Groups");
        }
    }
}
