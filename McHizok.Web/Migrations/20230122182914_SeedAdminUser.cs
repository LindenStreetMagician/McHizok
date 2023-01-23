using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace McHizok.Web.Migrations
{
    public partial class SeedAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "c08fecb6-e393-4a41-bb18-da5b83ca960b");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountFor", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "2c93d3ac-fde3-4193-b1a1-128da8fc0038", 0, "Myself", "3e176bd3-bd7c-4bd7-8f1b-d99f53685032", null, false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAECMuOGVV2/Xvqvnpjk4FrcJH2amkwejA11zphN25bkf9+WyQ94A6rNKPI9XKRTeWaQ==", null, false, "17c6f200-3c9a-4f8d-96e6-22fbc94464b6", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b383ab2f-0295-4194-bba9-7a870bfdd331", "2c93d3ac-fde3-4193-b1a1-128da8fc0038" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b383ab2f-0295-4194-bba9-7a870bfdd331", "2c93d3ac-fde3-4193-b1a1-128da8fc0038" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2c93d3ac-fde3-4193-b1a1-128da8fc0038");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b383ab2f-0295-4194-bba9-7a870bfdd331",
                column: "ConcurrencyStamp",
                value: "69933e0c-b1bb-435c-9835-d3c5d3092533");
        }
    }
}
