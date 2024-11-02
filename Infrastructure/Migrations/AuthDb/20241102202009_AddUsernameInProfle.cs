using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUsernameInProfle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6215f055-38d2-4723-a258-2ad0c7cd57ee"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a3f45cef-1f78-4715-8b19-f94b572bbab3"));

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("455973a2-5970-4a32-aa31-205d4b99415d"), null, "user", "USER" },
                    { new Guid("f54b7160-3a19-4c13-a6e9-19d28e608181"), null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("455973a2-5970-4a32-aa31-205d4b99415d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f54b7160-3a19-4c13-a6e9-19d28e608181"));

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Profiles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("6215f055-38d2-4723-a258-2ad0c7cd57ee"), null, "user", "USER" },
                    { new Guid("a3f45cef-1f78-4715-8b19-f94b572bbab3"), null, "admin", "ADMIN" }
                });
        }
    }
}
