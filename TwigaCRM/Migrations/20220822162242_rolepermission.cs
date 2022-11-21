using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class rolepermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRole_Permission_AppRole_AppRoleId1",
                table: "AppRole_Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_AppRole_Permission_Permission_PermissionId1",
                table: "AppRole_Permission");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AppRole_AppRoleId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AppRoleId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AppRole_Permission_AppRoleId1",
                table: "AppRole_Permission");

            migrationBuilder.DropIndex(
                name: "IX_AppRole_Permission_PermissionId1",
                table: "AppRole_Permission");

            migrationBuilder.DropColumn(
                name: "AppRoleId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppRoleId1",
                table: "AppRole_Permission");

            migrationBuilder.DropColumn(
                name: "PermissionId1",
                table: "AppRole_Permission");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6575fe80-666f-4815-b6c3-5ae47fb12d33", "SUPERADMIN@TWIGACRM.COM", "SUPERADMIN@TWIGACRM.COM", "AQAAAAEAACcQAAAAECV9RWFjQKYbZjRYbwYEVhfx9NzBq/uiOYkiUOqLggiHFOo6u3rGGXFoZPrm2hDneg==", "1fb59b6f-2242-4f11-8b1f-d25d29a95dc2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppRoleId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppRoleId1",
                table: "AppRole_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PermissionId1",
                table: "AppRole_Permission",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "44641c47-5bb4-4688-8eaa-0ea4e6911f37", null, null, "AQAAAAEAACcQAAAAEEhwVb/lvFRpGWRABuuhXvX1j0WZJrw44AXGLOnq+OoJM3GgJ0WHFq4ffeOpnQguJQ==", "f30ccfbf-33b6-4ef1-a5c8-a9722b42a62c" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AppRoleId1",
                table: "AspNetUsers",
                column: "AppRoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppRole_Permission_AppRoleId1",
                table: "AppRole_Permission",
                column: "AppRoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_AppRole_Permission_PermissionId1",
                table: "AppRole_Permission",
                column: "PermissionId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRole_Permission_AppRole_AppRoleId1",
                table: "AppRole_Permission",
                column: "AppRoleId1",
                principalTable: "AppRole",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRole_Permission_Permission_PermissionId1",
                table: "AppRole_Permission",
                column: "PermissionId1",
                principalTable: "Permission",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AppRole_AppRoleId1",
                table: "AspNetUsers",
                column: "AppRoleId1",
                principalTable: "AppRole",
                principalColumn: "Id");
        }
    }
}
