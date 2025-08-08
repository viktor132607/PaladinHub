using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PaladinHub.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_PostId",
                table: "DiscussionLikes");

            migrationBuilder.AddColumn<int>(
                name: "Likes",
                table: "DiscussionComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DiscussionCommentLikes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscussionCommentLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscussionCommentLikes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscussionCommentLikes_DiscussionComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "DiscussionComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_PostId_UserId",
                table: "DiscussionLikes",
                columns: new[] { "PostId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionCommentLikes_CommentId_UserId",
                table: "DiscussionCommentLikes",
                columns: new[] { "CommentId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionCommentLikes_UserId",
                table: "DiscussionCommentLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscussionCommentLikes");

            migrationBuilder.DropIndex(
                name: "IX_DiscussionLikes_PostId_UserId",
                table: "DiscussionLikes");

            migrationBuilder.DropColumn(
                name: "Likes",
                table: "DiscussionComments");

            migrationBuilder.CreateIndex(
                name: "IX_DiscussionLikes_PostId",
                table: "DiscussionLikes",
                column: "PostId");
        }
    }
}
