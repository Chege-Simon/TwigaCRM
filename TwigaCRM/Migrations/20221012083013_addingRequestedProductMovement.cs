using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingRequestedProductMovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ActualMovement",
                table: "RequestedProduct",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea2c2ddd-7adf-4215-955c-004bea82d9a3", "AQAAAAEAACcQAAAAEOjLPHtYLa58hBrrTg3TqgPTC9IUEIwGzUiMejScEvCdWTHKGP5ZLRc+gtJK+05DiQ==", "27f3d274-bda4-48f7-9ddf-32ee037d49c3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualMovement",
                table: "RequestedProduct");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c94a930f-fcfd-41cd-b547-2802e7557504", "AQAAAAEAACcQAAAAEMu5t5cJ3qcBVOYkqtwSCEDFXCMh6GNPanWMY3ZUzbsFt+eZQ7lu+YEZQN/JsSqCrQ==", "a52b4342-c977-4514-af20-097274abbab9" });
        }
    }
}
