using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class NewIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedOn_PopularityScore_UserId",
                table: "Posts",
                columns: new[] { "CreatedOn", "PopularityScore", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_PostReactions_PostId_CreatedOn",
                table: "PostReactions",
                columns: new[] { "PostId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreatedOn",
                table: "Comments",
                columns: new[] { "PostId", "CreatedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_CreatedOn_PopularityScore_UserId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_PostReactions_PostId_CreatedOn",
                table: "PostReactions");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_CreatedOn",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");
        }
    }
}
