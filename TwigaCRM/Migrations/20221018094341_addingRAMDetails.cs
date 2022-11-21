using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingRAMDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DailyAmount",
                table: "RAMSaleTargetMapping",
                newName: "ExpectedAmount");

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "RAMSaleTarget",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "RAMCollectionTarget",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22343724-4cbf-44d7-b1b7-2b8e763d9891", "AQAAAAEAACcQAAAAEGDrS54XBxywcrA3sWmaUbO0+oLbyTbz9ce8E8EbJlNurx+nwMCC6msZQ/36iL/4eA==", "327a7ecf-0cce-4c50-aac7-c4538cd45a23" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "RAMSaleTarget");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "RAMCollectionTarget");

            migrationBuilder.RenameColumn(
                name: "ExpectedAmount",
                table: "RAMSaleTargetMapping",
                newName: "DailyAmount");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "49b6b3e2-dd45-4bb6-b42c-c1b7dd871842", "AQAAAAEAACcQAAAAEMfahtenYKTZMDkSoG9h7WAmlSqsPiKMkEOWJ9REQjH/QML2yAFiaTLE03BdILdENA==", "3fb2e570-c4eb-4bf7-9d62-91dd341ad306" });
        }
    }
}
