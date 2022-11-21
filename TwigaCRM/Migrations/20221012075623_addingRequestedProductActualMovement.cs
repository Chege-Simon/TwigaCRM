using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingRequestedProductActualMovement : Migration
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
                values: new object[] { "8fb97fac-8781-40ca-9061-f215d69245fb", "AQAAAAEAACcQAAAAEMwIeX7PFj3uL/0b6qKagkbwIw7DicD5tuU5rfPgB7ThC+Udgi9ctJt5ul65tH5QKw==", "a6707bb3-c199-47be-875e-30ad8de84765" });
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
                values: new object[] { "9bea3cdc-099b-47cd-8b5f-a5113620de4f", "AQAAAAEAACcQAAAAEIBn9LtrxUR3Wj2Hab530AEPNMBOC9/piUIWZXqhB9H1um4fNNZg0zgYT++3do0haQ==", "68a8bd3a-ecac-4db2-9987-9744dc6c7871" });
        }
    }
}
