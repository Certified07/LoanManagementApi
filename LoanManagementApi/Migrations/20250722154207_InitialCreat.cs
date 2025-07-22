using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LoanManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "91f8dcdf-e48c-4ad2-8153-d6d787f71e74");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "44e36a50-0091-4f8f-84c6-e932cd9c4df2", "82b5947e-b56d-4662-90a7-39cdcd542e18" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44e36a50-0091-4f8f-84c6-e932cd9c4df2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "82b5947e-b56d-4662-90a7-39cdcd542e18");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Loans");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Repayments",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "RepaymentSchedule",
                table: "Loans",
                newName: "RepaymentType");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Repayments",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "RepaymentType",
                table: "LoanTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "Loans",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPaid",
                table: "Loans",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateOfBirth",
                table: "Clients",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Repayments");

            migrationBuilder.DropColumn(
                name: "RepaymentType",
                table: "LoanTypes");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "TotalPaid",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Repayments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "RepaymentType",
                table: "Loans",
                newName: "RepaymentSchedule");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Loans",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Loans",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "44e36a50-0091-4f8f-84c6-e932cd9c4df2", null, "Admin", null },
                    { "91f8dcdf-e48c-4ad2-8153-d6d787f71e74", null, "Client", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "82b5947e-b56d-4662-90a7-39cdcd542e18", 0, "83095f9f-6647-4746-9499-cccf144d8125", "admin@gmail.com", true, false, null, "ADMIN@GMAIL.COM", null, "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec", null, false, 0, "08c220f0-91a7-45a5-83f2-bd7386783787", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "44e36a50-0091-4f8f-84c6-e932cd9c4df2", "82b5947e-b56d-4662-90a7-39cdcd542e18" });
        }
    }
}
