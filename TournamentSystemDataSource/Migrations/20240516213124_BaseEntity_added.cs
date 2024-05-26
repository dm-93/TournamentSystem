using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentSystemDataSource.Migrations
{
    /// <inheritdoc />
    public partial class BaseEntity_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Tournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "TeamsDescriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "TeamsDescriptions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Teams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Prizes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Prizes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Persons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Matchups",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Matchups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Addresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "TeamsDescriptions");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "TeamsDescriptions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Prizes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Matchups");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Matchups");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Addresses");
        }
    }
}
