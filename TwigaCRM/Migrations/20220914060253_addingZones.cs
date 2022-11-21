using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingZones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmerType",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "Customer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Zone",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zone", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zone_Town_TownId",
                        column: x => x.TownId,
                        principalTable: "Town",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ebf5c73-e28f-4eab-9d3e-3dc340cdc6ff", "AQAAAAEAACcQAAAAENSjtfbAfiZUZ5z4A4PhPnAKai5H3zk7nbQ24o1CMaXWv3rZq6rZ3ebCgdZXxp0ZGw==", "e413d4b2-e4db-4599-a287-6c6880b9f315" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_ZoneId",
                table: "Customer",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ZoneId",
                table: "AspNetUsers",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Zone_TownId",
                table: "Zone",
                column: "TownId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Zone_ZoneId",
                table: "AspNetUsers",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Zone_ZoneId",
                table: "Customer",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Zone_ZoneId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Zone_ZoneId",
                table: "Customer");

            migrationBuilder.DropTable(
                name: "Zone");

            migrationBuilder.DropIndex(
                name: "IX_Customer_ZoneId",
                table: "Customer");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ZoneId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FarmerType",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2673663-6125-4fc5-bf43-6984a2acfa0b", "AQAAAAEAACcQAAAAEPaLmAbCCAKDZl69mA6Iy3sxcbpEx7ZgwuiX8P7vsmWgj6bz1f6Vi09dJX+ZYubomw==", "0c9061e0-bfea-40c0-a2cf-56fd85e98560" });
        }
    }
}
