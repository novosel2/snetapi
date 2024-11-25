using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class SetNullAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Profiles_FollowedId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Profiles_FollowerId",
                table: "Follows");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Profiles_FollowedId",
                table: "Follows",
                column: "FollowedId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Profiles_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Profiles_FollowedId",
                table: "Follows");

            migrationBuilder.DropForeignKey(
                name: "FK_Follows_Profiles_FollowerId",
                table: "Follows");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Profiles_FollowedId",
                table: "Follows",
                column: "FollowedId",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Follows_Profiles_FollowerId",
                table: "Follows",
                column: "FollowerId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
