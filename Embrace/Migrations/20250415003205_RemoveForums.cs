using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Embrace.Migrations
{
    /// <inheritdoc />
    public partial class RemoveForums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscussionBoards_Forums_ForumId",
                table: "DiscussionBoards");

            migrationBuilder.DropTable(
                name: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionBoards_ForumId",
                table: "DiscussionBoards");

            migrationBuilder.DropColumn(
                name: "ForumId",
                table: "DiscussionBoards");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "DiscussionBoards",
                newName: "Content");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "DiscussionBoards",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "ForumId",
                table: "DiscussionBoards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Forums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionBoards_ForumId",
                table: "DiscussionBoards",
                column: "ForumId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscussionBoards_Forums_ForumId",
                table: "DiscussionBoards",
                column: "ForumId",
                principalTable: "Forums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
