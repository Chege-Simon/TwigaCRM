using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class SaleMovementTargetCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "SalesMovement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "987ceba0-a05c-492a-b977-86612bdba564", "AQAAAAEAACcQAAAAED1ACVh2vlNS/Lsrp9V73K6LphsWESS3O2cNTpXkcU58vlhpEdv2jJF4F8FNbti9QA==", "cf293764-bea4-4b16-aaa4-d4867cc618e2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SalesMovement");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "22343724-4cbf-44d7-b1b7-2b8e763d9891", "AQAAAAEAACcQAAAAEGDrS54XBxywcrA3sWmaUbO0+oLbyTbz9ce8E8EbJlNurx+nwMCC6msZQ/36iL/4eA==", "327a7ecf-0cce-4c50-aac7-c4538cd45a23" });
        }
    }
}
