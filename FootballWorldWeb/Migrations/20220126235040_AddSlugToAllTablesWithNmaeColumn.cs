using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballWorldWeb.Migrations
{
    public partial class AddSlugToAllTablesWithNmaeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Teams",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Seasons",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Groups",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Competitions",
                type: "varchar(255)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Seasons");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Competitions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Groups",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);
        }
    }
}
