using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Baum.DB.Migrations
{
    /// <inheritdoc />
    public partial class CreateSoundChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInherited",
                table: "Words",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Words",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SoundChanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageId = table.Column<int>(type: "INTEGER", nullable: false),
                    Notation = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoundChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoundChanges_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_ParentId",
                table: "Words",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SoundChanges_LanguageId",
                table: "SoundChanges",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_Words_ParentId",
                table: "Words",
                column: "ParentId",
                principalTable: "Words",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_Words_ParentId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "SoundChanges");

            migrationBuilder.DropIndex(
                name: "IX_Words_ParentId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "IsInherited",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Words");
        }
    }
}
