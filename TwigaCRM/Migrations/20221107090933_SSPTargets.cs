using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class SSPTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TargetCustomerId",
                table: "Target",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28efb83c-175c-478a-9afc-02f6c2fbb4c4", "AQAAAAEAACcQAAAAEEzY/J6UZFPEeDnWCpHz6IE/aB7UNheXunovULnSU8Ss+2xjF2qnVayUUZCwWn4CYg==", "955bfde3-a344-4be1-ba77-37d6c49478ce" });

            migrationBuilder.CreateIndex(
                name: "IX_Target_TargetCustomerId",
                table: "Target",
                column: "TargetCustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Target_Customer_TargetCustomerId",
                table: "Target",
                column: "TargetCustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Target_Customer_TargetCustomerId",
                table: "Target");

            migrationBuilder.DropIndex(
                name: "IX_Target_TargetCustomerId",
                table: "Target");

            migrationBuilder.DropColumn(
                name: "TargetCustomerId",
                table: "Target");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "987ceba0-a05c-492a-b977-86612bdba564", "AQAAAAEAACcQAAAAED1ACVh2vlNS/Lsrp9V73K6LphsWESS3O2cNTpXkcU58vlhpEdv2jJF4F8FNbti9QA==", "cf293764-bea4-4b16-aaa4-d4867cc618e2" });
        }
    }
}
