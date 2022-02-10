using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballWorldWeb.Migrations
{
    public partial class AddGroupTypeToGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupType",
                table: "Groups",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupType",
                table: "Groups");
        }
    }
}
