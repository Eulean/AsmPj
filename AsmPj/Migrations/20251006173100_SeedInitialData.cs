using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsmPj.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "Created", "Description", "IsActive", "IsDeleted", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3672), null, true, false, null, "Dashboard" },
                    { 2, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3675), null, true, false, null, "Users" },
                    { 3, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3676), null, true, false, null, "Roles" },
                    { 4, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3677), null, true, false, null, "Reports" }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Created", "Description", "IsActive", "IsDeleted", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3633), null, true, false, null, "ViewDashboard" },
                    { 2, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3637), null, true, false, null, "ManageUsers" },
                    { 3, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3668), null, true, false, null, "ManageRoles" },
                    { 4, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3669), null, true, false, null, "ViewReports" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Created", "Description", "IsActive", "IsDeleted", "Modified", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3622), "Full system access", true, false, null, "Admin" },
                    { 2, new DateTime(2025, 10, 6, 17, 30, 59, 370, DateTimeKind.Utc).AddTicks(3626), "Standard user access", true, false, null, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateCreated", "DateModified", "Email", "IsActive", "IsDeleted", "Name", "Password", "Role" },
                values: new object[] { 1, new DateTime(2025, 10, 6, 17, 30, 59, 623, DateTimeKind.Utc).AddTicks(1056), null, "nightruner115@gmail.com", true, false, "System Admin", "$2a$11$9Ye6qFmlf/Oiamke45bB3OHwApcfkB/D0kP5Z723liQTbAdWKGYRK", "Admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
