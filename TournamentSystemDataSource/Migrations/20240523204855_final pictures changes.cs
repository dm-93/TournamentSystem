using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentSystemDataSource.Migrations
{
    /// <inheritdoc />
    public partial class finalpictureschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentPictureId",
                table: "Tournaments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamPictureId",
                table: "TeamsDescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserPictureId",
                table: "Persons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentPictureId",
                table: "Tournaments",
                column: "TournamentPictureId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsDescriptions_TeamPictureId",
                table: "TeamsDescriptions",
                column: "TeamPictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_UserPictureId",
                table: "Persons",
                column: "UserPictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Persons_Pictures_UserPictureId",
                table: "Persons",
                column: "UserPictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamsDescriptions_Pictures_TeamPictureId",
                table: "TeamsDescriptions",
                column: "TeamPictureId",
                principalTable: "Pictures",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_Pictures_TournamentPictureId",
                table: "Tournaments",
                column: "TournamentPictureId",
                principalTable: "Pictures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Pictures_UserPictureId",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamsDescriptions_Pictures_TeamPictureId",
                table: "TeamsDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_Pictures_TournamentPictureId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_TournamentPictureId",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_TeamsDescriptions_TeamPictureId",
                table: "TeamsDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Persons_UserPictureId",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "TournamentPictureId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TeamPictureId",
                table: "TeamsDescriptions");

            migrationBuilder.DropColumn(
                name: "UserPictureId",
                table: "Persons");
        }
    }
}
