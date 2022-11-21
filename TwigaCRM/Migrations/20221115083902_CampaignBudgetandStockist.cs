using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class CampaignBudgetandStockist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignBudget_Town_TownId",
                table: "CampaignBudget");

            migrationBuilder.DropColumn(
                name: "NumberOfPromoters",
                table: "Campaign");

            migrationBuilder.RenameColumn(
                name: "NumberOfCampaigns",
                table: "CampaignBudget",
                newName: "Month");

            migrationBuilder.AlterColumn<int>(
                name: "TownId",
                table: "CampaignBudget",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BusinessLineId",
                table: "CampaignBudget",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                values: new object[] { "f4445805-b4a1-4470-94f7-1f12ac6ed936", "AQAAAAEAACcQAAAAECipSdW/3ONVMXEKZ8Hf5B6l55gRFR+08nvRfbigdpA3uE9wGWzVjgo6jzu9318CBg==", "25263985-1cbd-4054-ac47-a6e98d5adc78" });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignBudget_BusinessLineId",
                table: "CampaignBudget",
                column: "BusinessLineId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignBudget_BusinessLine_BusinessLineId",
                table: "CampaignBudget",
                column: "BusinessLineId",
                principalTable: "BusinessLine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignBudget_Town_TownId",
                table: "CampaignBudget",
                column: "TownId",
                principalTable: "Town",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_Customer_PartneringStockistId",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignBudget_BusinessLine_BusinessLineId",
                table: "CampaignBudget");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignBudget_Town_TownId",
                table: "CampaignBudget");

            migrationBuilder.DropIndex(
                name: "IX_CampaignBudget_BusinessLineId",
                table: "CampaignBudget");

            migrationBuilder.DropIndex(
                name: "IX_Campaign_PartneringStockistId",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "BusinessLineId",
                table: "CampaignBudget");

            migrationBuilder.DropColumn(
                name: "PartneringStockistId",
                table: "Campaign");

            migrationBuilder.RenameColumn(
                name: "Month",
                table: "CampaignBudget",
                newName: "NumberOfCampaigns");

            migrationBuilder.AlterColumn<int>(
                name: "TownId",
                table: "CampaignBudget",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfPromoters",
                table: "Campaign",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2c3402e9-a697-4788-bb0f-9006ae41e59a", "AQAAAAEAACcQAAAAEIKrLpx5qVBcsYu+BnxfovBCBnf98cAJ8Ltg1NVnEJiWg26f2hjQmG3K1kpvnag8mw==", "8fb64158-c428-4bec-9067-e04e9350527d" });

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignBudget_Town_TownId",
                table: "CampaignBudget",
                column: "TownId",
                principalTable: "Town",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
