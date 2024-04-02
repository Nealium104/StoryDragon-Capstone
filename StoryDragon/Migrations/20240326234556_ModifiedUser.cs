using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryDragon.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostCharacters");

            migrationBuilder.DropTable(
                name: "PostStories");

            migrationBuilder.AddColumn<Guid>(
                name: "CharacterId",
                table: "Posts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostType",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StoryId",
                table: "Posts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_StoryId",
                table: "Posts",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "CharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Stories_StoryId",
                table: "Posts",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Characters_CharacterId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Stories_StoryId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CharacterId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_StoryId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CharacterId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PostType",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "PostCharacters",
                columns: table => new
                {
                    CharactersCharacterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PostsPostId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCharacters", x => new { x.CharactersCharacterId, x.PostsPostId });
                    table.ForeignKey(
                        name: "FK_PostCharacters_Characters_CharactersCharacterId",
                        column: x => x.CharactersCharacterId,
                        principalTable: "Characters",
                        principalColumn: "CharacterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCharacters_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostStories",
                columns: table => new
                {
                    PostsPostId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoriesStoryID = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostStories", x => new { x.PostsPostId, x.StoriesStoryID });
                    table.ForeignKey(
                        name: "FK_PostStories_Posts_PostsPostId",
                        column: x => x.PostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostStories_Stories_StoriesStoryID",
                        column: x => x.StoriesStoryID,
                        principalTable: "Stories",
                        principalColumn: "StoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostCharacters_PostsPostId",
                table: "PostCharacters",
                column: "PostsPostId");

            migrationBuilder.CreateIndex(
                name: "IX_PostStories_StoriesStoryID",
                table: "PostStories",
                column: "StoriesStoryID");
        }
    }
}
