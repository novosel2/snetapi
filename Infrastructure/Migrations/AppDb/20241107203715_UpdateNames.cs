using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AppDb
{
    /// <inheritdoc />
    public partial class UpdateNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Profiles_ProfileId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ProfileId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Profiles_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Profiles_UserId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_UserId",
                table: "Comment");

            migrationBuilder.AddColumn<Guid>(
                name: "ProfileId",
                table: "Comment",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ProfileId",
                table: "Comment",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Profiles_ProfileId",
                table: "Comment",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }
    }
}
