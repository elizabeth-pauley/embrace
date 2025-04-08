using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDataFromDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentData",
                table: "Documents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "DocumentData",
                table: "Documents",
                type: "longblob",
                nullable: false);
        }
    }
}
