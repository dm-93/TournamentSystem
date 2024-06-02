using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentSystemDataSource.Migrations
{
    /// <inheritdoc />
    public partial class Completedcolumnadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchupEntries_Matchups_ParentMatchupId",
                table: "MatchupEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchupEntries_Teams_TeamCompetingId",
                table: "MatchupEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchups_Teams_WinnerId",
                table: "Matchups");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchups_Tournaments_TournamentId",
                table: "Matchups");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Teams_TeamId",
                table: "Persons");

            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Persons",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Matchups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentMatchupId",
                table: "MatchupEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchupEntries_Matchups_ParentMatchupId",
                table: "MatchupEntries",
                column: "ParentMatchupId",
                principalTable: "Matchups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchupEntries_Teams_TeamCompetingId",
                table: "MatchupEntries",
                column: "TeamCompetingId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchups_Teams_WinnerId",
                table: "Matchups",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchups_Tournaments_TournamentId",
                table: "Matchups",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Teams_TeamId",
                table: "Persons",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchupEntries_Matchups_ParentMatchupId",
                table: "MatchupEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchupEntries_Teams_TeamCompetingId",
                table: "MatchupEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchups_Teams_WinnerId",
                table: "Matchups");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchups_Tournaments_TournamentId",
                table: "Matchups");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Teams_TeamId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Tournaments");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TournamentId",
                table: "Matchups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ParentMatchupId",
                table: "MatchupEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchupEntries_Matchups_ParentMatchupId",
                table: "MatchupEntries",
                column: "ParentMatchupId",
                principalTable: "Matchups",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchupEntries_Teams_TeamCompetingId",
                table: "MatchupEntries",
                column: "TeamCompetingId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchups_Teams_WinnerId",
                table: "Matchups",
                column: "WinnerId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchups_Tournaments_TournamentId",
                table: "Matchups",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Teams_TeamId",
                table: "Persons",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
