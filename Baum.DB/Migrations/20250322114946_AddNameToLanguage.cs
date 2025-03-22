using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baum.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToLanguage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Languages",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Languages");
        }
    }
}
