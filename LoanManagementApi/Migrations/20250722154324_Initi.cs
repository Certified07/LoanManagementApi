using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class Initi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e66c636a-fac3-4129-8db4-1a5fdf885271");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "fe8ed3c0-af33-49ba-8833-63b228870023", "acdfbe5d-7396-44be-bb98-58a9c2d3358c" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe8ed3c0-af33-49ba-8833-63b228870023");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "acdfbe5d-7396-44be-bb98-58a9c2d3358c");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "70a047c8-32ae-4e24-bf21-433e17b9248c", null, "Client", null },
                    { "ec392658-5c6a-4335-853a-8c79c42d3bc5", null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "db8c9dec-50e1-4905-90c4-2b519dfa11c0", 0, "26b26a44-cdea-4ae3-beba-0cc853481acc", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", null, "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec", null, false, 0, "ebf0ada2-9313-4a53-821c-c29ea7998e7b", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "ec392658-5c6a-4335-853a-8c79c42d3bc5", "db8c9dec-50e1-4905-90c4-2b519dfa11c0" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "70a047c8-32ae-4e24-bf21-433e17b9248c");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ec392658-5c6a-4335-853a-8c79c42d3bc5", "db8c9dec-50e1-4905-90c4-2b519dfa11c0" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec392658-5c6a-4335-853a-8c79c42d3bc5");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "db8c9dec-50e1-4905-90c4-2b519dfa11c0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "e66c636a-fac3-4129-8db4-1a5fdf885271", null, "Client", null },
                    { "fe8ed3c0-af33-49ba-8833-63b228870023", null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "acdfbe5d-7396-44be-bb98-58a9c2d3358c", 0, "51fc49a5-3407-4b2a-97e9-b965d6a5e618", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", null, "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec", null, false, 0, "acc7d811-c5b0-4244-a3ca-6a5bb0acdd3a", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "fe8ed3c0-af33-49ba-8833-63b228870023", "acdfbe5d-7396-44be-bb98-58a9c2d3358c" });
        }
    }
}
