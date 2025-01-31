using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8174df12-eaeb-43b3-8c7e-dab6a38870f8"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("9b3320d4-d61d-4ded-a75b-4f8adbd98295"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("02a6e62e-6c01-4fbc-a04d-84cd82892f3d"), null, "user", "USER" },
                    { new Guid("c5db6645-617a-4476-a35a-cadb2648836e"), null, "admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("02a6e62e-6c01-4fbc-a04d-84cd82892f3d"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c5db6645-617a-4476-a35a-cadb2648836e"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8174df12-eaeb-43b3-8c7e-dab6a38870f8"), null, "admin", "ADMIN" },
                    { new Guid("9b3320d4-d61d-4ded-a75b-4f8adbd98295"), null, "user", "USER" }
                });
        }
    }
}
