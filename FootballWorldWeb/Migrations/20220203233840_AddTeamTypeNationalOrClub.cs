using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballWorldWeb.Migrations
{
    public partial class AddTeamTypeNationalOrClub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamType",
                table: "Teams",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamType",
                table: "Teams");
        }
    }
}
