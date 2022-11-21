using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class CampaignPartneringStockists : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartneringStockistId",
                table: "Campaign",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4f406730-2048-4624-a738-9e0c4f13feca", "AQAAAAEAACcQAAAAEBQ7S4xwY7NEtegPTj/Qd4ajiVDZddCHs0hOfMIAG6JWUmAZ47hmnU977gJ2iYyyEQ==", "ca96b0ba-99c6-49eb-9539-c2fa349db325" });

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_PartneringStockistId",
                table: "Campaign",
                column: "PartneringStockistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_Customer_PartneringStockistId",
                table: "Campaign",
                column: "PartneringStockistId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Customer_PartneringStockistId",
                table: "Campaign");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_PartneringStockistId",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "PartneringStockistId",
                table: "Campaign");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b261dc55-291b-4d54-9c43-f54bd67bcc1d", "AQAAAAEAACcQAAAAEF/5uzh9FniLWCh1pxp6Am9PfnHQumcVBS0wwKucIFWECo9SJX+QRykDnTRBelzFDQ==", "c5030216-677f-48b7-aaea-ae429f5ce0e9" });
        }
    }
}
