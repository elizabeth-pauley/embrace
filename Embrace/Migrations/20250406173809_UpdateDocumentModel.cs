using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocumentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TranslatedDataPath",
                table: "Documents",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "OriginalDataPath",
                table: "Documents",
                newName: "FileName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Documents",
                newName: "TranslatedDataPath");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Documents",
                newName: "OriginalDataPath");
        }
    }
}
