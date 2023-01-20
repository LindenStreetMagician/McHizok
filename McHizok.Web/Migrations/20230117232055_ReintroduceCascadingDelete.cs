using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHizok.Web.Migrations
{
    public partial class ReintroduceCascadingDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
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
                value: "69933e0c-b1bb-435c-9835-d3c5d3092533");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponInventory_AspNetUsers_UserId",
                table: "CouponInventory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
