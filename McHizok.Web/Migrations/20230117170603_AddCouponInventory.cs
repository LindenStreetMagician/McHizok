using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHizok.Web.Migrations
{
    public partial class AddCouponInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CouponInventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CouponBase64 = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CouponCode = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UserId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponInventory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "11e5295d-23f5-49d1-9018-9028b37901ae");

            migrationBuilder.CreateIndex(
                name: "IX_CouponInventory_UserId",
                table: "CouponInventory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponInventory");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "a6dd8953-7979-4374-bbc1-0d3a0d9aae7d");
        }
    }
}
