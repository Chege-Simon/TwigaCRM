using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class VSARoutePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "RAMPlan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Plan",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c3402e9-a697-4788-bb0f-9006ae41e59a", "AQAAAAEAACcQAAAAEIKrLpx5qVBcsYu+BnxfovBCBnf98cAJ8Ltg1NVnEJiWg26f2hjQmG3K1kpvnag8mw==", "8fb64158-c428-4bec-9067-e04e9350527d" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "RAMPlan");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Plan");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "29fc7f0d-7222-4d0f-bba8-039556ac1f9a", "AQAAAAEAACcQAAAAEFI0QtEBZvQlg4SicChqY+sTA6/dJ9l53bre7gEn4G+MtJz919Yv5ejoxcrcQvHLyw==", "ddaa9e1f-9f2d-486d-9a36-46f3a8d3d9cd" });
        }
    }
}
