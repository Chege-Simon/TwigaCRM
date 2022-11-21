using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingFOAApproval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFOAApproved",
                table: "RequestedProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFOAApproved",
                table: "RequestedCampaignItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2673663-6125-4fc5-bf43-6984a2acfa0b", "AQAAAAEAACcQAAAAEPaLmAbCCAKDZl69mA6Iy3sxcbpEx7ZgwuiX8P7vsmWgj6bz1f6Vi09dJX+ZYubomw==", "0c9061e0-bfea-40c0-a2cf-56fd85e98560" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFOAApproved",
                table: "RequestedProduct");

            migrationBuilder.DropColumn(
                name: "IsFOAApproved",
                table: "RequestedCampaignItem");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1fce38e8-7b97-453c-a7c0-90ef7418e19a", "AQAAAAEAACcQAAAAEKSy44gJy6ZdufCyTaaBi9EGaLW3cDIz6OQn4YfMHDTB7lBadGAvCeEyyJwUiCwzvw==", "c26cd9b6-a208-4903-a56c-a1c55d96f66a" });
        }
    }
}
