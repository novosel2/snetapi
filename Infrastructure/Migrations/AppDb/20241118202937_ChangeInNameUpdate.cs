using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class ChangeInNameUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Profiles_RecieverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Profiles_RecieverId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "Friendships",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SenderId_RecieverId",
                table: "Friendships",
                newName: "IX_Friendships_SenderId_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_RecieverId",
                table: "Friendships",
                newName: "IX_Friendships_ReceiverId");

            migrationBuilder.RenameColumn(
                name: "RecieverId",
                table: "FriendRequests",
                newName: "ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_SenderId_RecieverId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_SenderId_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_RecieverId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_ReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Profiles_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Profiles_ReceiverId",
                table: "Friendships",
                column: "ReceiverId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendRequests_Profiles_ReceiverId",
                table: "FriendRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_Profiles_ReceiverId",
                table: "Friendships");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Friendships",
                newName: "RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_SenderId_ReceiverId",
                table: "Friendships",
                newName: "IX_Friendships_SenderId_RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_ReceiverId",
                table: "Friendships",
                newName: "IX_Friendships_RecieverId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "FriendRequests",
                newName: "RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_SenderId_ReceiverId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_SenderId_RecieverId");

            migrationBuilder.RenameIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                newName: "IX_FriendRequests_RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendRequests_Profiles_RecieverId",
                table: "FriendRequests",
                column: "RecieverId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_Profiles_RecieverId",
                table: "Friendships",
                column: "RecieverId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
