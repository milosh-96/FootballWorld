using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace FootballWorldWeb.Migrations
{
    public partial class AddStandingsDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Standings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GroupId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standings_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandingRows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StandingsId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Played = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Draws = table.Column<int>(nullable: false),
                    Losses = table.Column<int>(nullable: false),
                    GoalsScored = table.Column<int>(nullable: false),
                    GoalsConceded = table.Column<int>(nullable: false),
                    HomeWins = table.Column<int>(nullable: false),
                    HomeGoals = table.Column<int>(nullable: false),
                    AwayWins = table.Column<int>(nullable: false),
                    AwayGoals = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandingRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandingRows_Standings_StandingsId",
                        column: x => x.StandingsId,
                        principalTable: "Standings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandingRows_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StandingRows_StandingsId",
                table: "StandingRows",
                column: "StandingsId");

            migrationBuilder.CreateIndex(
                name: "IX_StandingRows_TeamId",
                table: "StandingRows",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Standings_GroupId",
                table: "Standings",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StandingRows");

            migrationBuilder.DropTable(
                name: "Standings");
        }
    }
}
