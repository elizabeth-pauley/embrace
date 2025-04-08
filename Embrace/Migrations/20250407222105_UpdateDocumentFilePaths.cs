using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocumentFilePaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Documents",
                newName: "UploadedFilePath");

            migrationBuilder.AddColumn<string>(
                name: "TranslatedFilePath",
                table: "Documents",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TranslatedFilePath",
                table: "Documents");

            migrationBuilder.RenameColumn(
                name: "UploadedFilePath",
                table: "Documents",
                newName: "FilePath");
        }
    }
}
