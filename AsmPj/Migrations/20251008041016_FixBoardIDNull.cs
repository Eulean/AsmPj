using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsmPj.Migrations
{
    /// <inheritdoc />
    public partial class FixBoardIDNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BordId",
                table: "TaskItems");

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5925));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5928));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5929));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5930));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5916));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5920));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5921));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5922));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 8, 4, 10, 15, 601, DateTimeKind.Utc).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2025, 10, 8, 4, 10, 15, 850, DateTimeKind.Utc).AddTicks(518), "$2a$11$fpbqrT1fDDRuX8GEgI0MCezywAZxjsUobDPbW8jPLfS.C1TV3pjDa" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BordId",
                table: "TaskItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5812));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5821));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5822));

            migrationBuilder.UpdateData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5823));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5796));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5808));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5809));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5777));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2025, 10, 7, 5, 44, 54, 718, DateTimeKind.Utc).AddTicks(5780));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "Password" },
                values: new object[] { new DateTime(2025, 10, 7, 5, 44, 54, 987, DateTimeKind.Utc).AddTicks(8776), "$2a$11$ozZGm0Jnv4bsbB5oHw1ozOW9vKTgjkgrOrEtrbjf2WQc5hCNlMZtG" });
        }
    }
}
