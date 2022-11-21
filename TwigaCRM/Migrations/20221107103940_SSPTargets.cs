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
                values: new object[] { "63e08e6c-4f51-497d-81fc-2df2fbb2c179", "AQAAAAEAACcQAAAAEGd8x4ijWoCksBMU1UtLWlpQAMLJdZYD0mCzlaB4ASWi/4aomvgPq/av14GbcQn3wQ==", "499fb007-3199-48b2-939f-af917d7701c5" });

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
                values: new object[] { "0be45bfe-01c5-4e84-8dea-03040776eb2f", "AQAAAAEAACcQAAAAEJozzysp8mJeU9qIMxhhf1/ne6U4nGOHxiB5FzH6+S61uSE7xRFxq66iyEQi2GQhyA==", "04ffcd31-960b-415f-ac5c-cb2a07438230" });
        }
    }
}
