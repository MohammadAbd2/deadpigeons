using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efscaffold.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminBoardHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "adminid",
                schema: "deadpigeons",
                table: "adminboardhistory");

            migrationBuilder.DropColumn(
                name: "iswinner",
                schema: "deadpigeons",
                table: "adminboardhistory");

            migrationBuilder.AddColumn<int>(
                name: "totalwinners",
                schema: "deadpigeons",
                table: "adminboardhistory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string[]>(
                name: "winningusers",
                schema: "deadpigeons",
                table: "adminboardhistory",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalwinners",
                schema: "deadpigeons",
                table: "adminboardhistory");

            migrationBuilder.DropColumn(
                name: "winningusers",
                schema: "deadpigeons",
                table: "adminboardhistory");

            migrationBuilder.AddColumn<string>(
                name: "adminid",
                schema: "deadpigeons",
                table: "adminboardhistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "iswinner",
                schema: "deadpigeons",
                table: "adminboardhistory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
