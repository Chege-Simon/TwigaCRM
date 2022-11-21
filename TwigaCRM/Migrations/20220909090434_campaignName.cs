using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class campaignName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1fce38e8-7b97-453c-a7c0-90ef7418e19a", "AQAAAAEAACcQAAAAEKSy44gJy6ZdufCyTaaBi9EGaLW3cDIz6OQn4YfMHDTB7lBadGAvCeEyyJwUiCwzvw==", "c26cd9b6-a208-4903-a56c-a1c55d96f66a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Campaign");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c899df0f-4269-4baa-aef2-74c359ee9ede", "AQAAAAEAACcQAAAAEA52/s4uJAB1RHJ7B/G+i8nNU3yO/RPePazWE/JT+lnhZ4B42f67dpgWEzbh3par7A==", "f5aae94e-75c7-4a54-a2e3-58cf69e03fd0" });
        }
    }
}
