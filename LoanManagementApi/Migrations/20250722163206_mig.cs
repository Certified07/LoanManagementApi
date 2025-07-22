using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                    { "3f679a8f-6105-4c75-9e78-2261e12ec0e5", null, "Client", null },
                    { "f94a8fc1-1742-40dd-96c2-a1515568a26f", null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "6eb0c335-4cfd-40f6-a5f9-39db6bebbc43", 0, "a9e0f276-e130-4191-9bd4-e4e85ce928bb", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", null, "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec", null, false, 0, "17f6b818-6d33-447e-a56b-11d1908205c9", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "f94a8fc1-1742-40dd-96c2-a1515568a26f", "6eb0c335-4cfd-40f6-a5f9-39db6bebbc43" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f679a8f-6105-4c75-9e78-2261e12ec0e5");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "f94a8fc1-1742-40dd-96c2-a1515568a26f", "6eb0c335-4cfd-40f6-a5f9-39db6bebbc43" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f94a8fc1-1742-40dd-96c2-a1515568a26f");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "6eb0c335-4cfd-40f6-a5f9-39db6bebbc43");

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
    }
}
