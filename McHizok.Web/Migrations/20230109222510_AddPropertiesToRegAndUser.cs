using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHizok.Web.Migrations
{
    public partial class AddPropertiesToRegAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "To",
                table: "Registration",
                newName: "AccountFor");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Registration",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "AccountFor",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "a6dd8953-7979-4374-bbc1-0d3a0d9aae7d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Registration");

            migrationBuilder.DropColumn(
                name: "AccountFor",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AccountFor",
                table: "Registration",
                newName: "To");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "b708b7eb-de52-48fd-8985-778e30dc998c");
        }
    }
}
