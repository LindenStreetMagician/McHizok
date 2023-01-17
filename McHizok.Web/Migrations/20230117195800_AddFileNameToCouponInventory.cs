using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHizok.Web.Migrations
{
    public partial class AddFileNameToCouponInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
                table: "CouponInventory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CouponInventory",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "CouponInventory",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "27a87720-0df5-4ba7-9286-3b1d1d7d1ac2");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
                table: "CouponInventory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
                table: "CouponInventory");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "CouponInventory");

            migrationBuilder.UpdateData(
                table: "CouponInventory",
                keyColumn: "UserId",
                keyValue: null,
                column: "UserId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "CouponInventory",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "11e5295d-23f5-49d1-9018-9028b37901ae");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
                table: "CouponInventory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
