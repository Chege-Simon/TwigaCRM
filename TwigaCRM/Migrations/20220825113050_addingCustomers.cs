using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingCustomers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactPersonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_Town_TownId",
                        column: x => x.TownId,
                        principalTable: "Town",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerBusinessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    BusinessLineId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerBusinessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerBusinessLine_BusinessLine_BusinessLineId",
                        column: x => x.BusinessLineId,
                        principalTable: "BusinessLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerBusinessLine_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSector_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSector_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "067c0778-eddc-4c62-aa6c-c39c256cf6a4", "AQAAAAEAACcQAAAAEKw75C5KUsbJoeKG0RH9tc1b7DK8oFXPK69Dunl9GL60EXqbKQfibYHFJeCsm1YLzw==", "e981167d-8b0d-4b32-b383-722a1c886a92" });

            migrationBuilder.CreateIndex(
                name: "IX_Customer_TownId",
                table: "Customer",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBusinessLine_BusinessLineId",
                table: "CustomerBusinessLine",
                column: "BusinessLineId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerBusinessLine_CustomerId",
                table: "CustomerBusinessLine",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSector_CustomerId",
                table: "CustomerSector",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSector_SectorId",
                table: "CustomerSector",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerBusinessLine");

            migrationBuilder.DropTable(
                name: "CustomerSector");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "16c4bbe0-9c76-457f-a66d-42b92bc1390e", "AQAAAAEAACcQAAAAELxbk4AoaVvaPOWNLMoZbEGwOG3yzZhZINryFG1O6YRBBXyWbBT8H8rkSzwCMuXJGw==", "3fb4f3cf-c791-4d52-8e74-6f5d83bb89ae" });
        }
    }
}
