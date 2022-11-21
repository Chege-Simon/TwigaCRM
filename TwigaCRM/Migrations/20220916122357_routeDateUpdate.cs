using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class routeDateUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Route");

            migrationBuilder.AddColumn<DateTime>(
                name: "RouteDate",
                table: "Route",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8602039-bad1-478c-84e9-b381c27e7123", "AQAAAAEAACcQAAAAELLCzqEn4gbXFTFq0v9LR5Mhi5LjQQ1l4kEsfXLaPvJimhcV7e8RduvKNZb4xVyOeA==", "5f7ff6c4-3ffe-4a61-97a7-a05b07552754" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RouteDate",
                table: "Route");

            migrationBuilder.AddColumn<string>(
                name: "Date",
                table: "Route",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e746d81-35c1-4cf2-b70a-624fde805713", "AQAAAAEAACcQAAAAEEcHdPHccZfYeTO6yNpqwaOAJabFg20cPWOdqcDbPkbhhd5Zw6POPivZeRud1SKnTw==", "7eff6481-7efe-4725-a708-23b8799d39f2" });
        }
    }
}
