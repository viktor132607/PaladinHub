using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaladinHub.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscussionLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscussionLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionLikes_DiscussionPosts_PostId",
                        column: x => x.PostId,
                        principalTable: "DiscussionPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_PostId",
                table: "DiscussionLikes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_UserId",
                table: "DiscussionLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscussionLikes");
        }
    }
}
