using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStorageOfUsersInDocsAndBoards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionBoards_AspNetUsers_UserId",
                table: "DiscussionBoards");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionBoards_UserId",
                table: "DiscussionBoards");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "DiscussionBoards",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "DiscussionBoards",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionBoards_UserId",
                table: "DiscussionBoards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionBoards_AspNetUsers_UserId",
                table: "DiscussionBoards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
