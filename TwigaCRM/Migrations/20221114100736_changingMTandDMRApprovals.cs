using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class changingMTandDMRApprovals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HRMstatus",
                table: "SalesMovement",
                newName: "TLstatus");

            migrationBuilder.RenameColumn(
                name: "HRMstatus",
                table: "DailyMovementReport",
                newName: "TLstatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29fc7f0d-7222-4d0f-bba8-039556ac1f9a", "AQAAAAEAACcQAAAAEFI0QtEBZvQlg4SicChqY+sTA6/dJ9l53bre7gEn4G+MtJz919Yv5ejoxcrcQvHLyw==", "ddaa9e1f-9f2d-486d-9a36-46f3a8d3d9cd" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TLstatus",
                table: "SalesMovement",
                newName: "HRMstatus");

            migrationBuilder.RenameColumn(
                name: "TLstatus",
                table: "DailyMovementReport",
                newName: "HRMstatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "278bdf78-ca89-4f09-9360-46d75146f956", "AQAAAAEAACcQAAAAEM6vCE6ROWNKYDHUwRser8NiWIE5IqM2ner1Xb0dg6LmIItLFlQxQVkZOx5PusuUDw==", "e8907533-8e2d-4de7-8222-7f558b25a547" });
        }
    }
}
