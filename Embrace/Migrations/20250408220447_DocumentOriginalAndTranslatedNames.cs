using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class DocumentOriginalAndTranslatedNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "UploadedFileName");

            migrationBuilder.AddColumn<string>(
                name: "TranslatedFileName",
                table: "Documents",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranslatedFileName",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "UploadedFileName",
                table: "Documents",
                newName: "FileName");
        }
    }
}
