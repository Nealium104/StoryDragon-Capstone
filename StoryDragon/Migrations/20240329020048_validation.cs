using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryDragon.Migrations
{
    /// <inheritdoc />
    public partial class validation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ValidateStoryOrCharacter",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidateStoryOrCharacter",
                table: "Posts");
        }
    }
}
