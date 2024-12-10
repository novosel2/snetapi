using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class MoreIndexing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId_CreatedOn",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId_PostId_CreatedOn",
                table: "Comments",
                columns: new[] { "UserId", "PostId", "CreatedOn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_UserId_PostId_CreatedOn",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId_CreatedOn",
                table: "Comments",
                columns: new[] { "PostId", "CreatedOn" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }
    }
}
