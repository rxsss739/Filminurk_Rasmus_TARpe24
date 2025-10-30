using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filminurk.Data.Migrations
{
    /// <inheritdoc />
    public partial class actors4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EntryCreatedAt",
                table: "Actors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EntryModifiedAt",
                table: "Actors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntryCreatedAt",
                table: "Actors");

            migrationBuilder.DropColumn(
                name: "EntryModifiedAt",
                table: "Actors");
        }
    }
}
