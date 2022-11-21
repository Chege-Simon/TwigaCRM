using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingStockTake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTake",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RetailAccountManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    StockTakeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTake", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTake_AspNetUsers_RetailAccountManagerId",
                        column: x => x.RetailAccountManagerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockTake_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTake_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7d47e71b-dd1b-448e-aaf4-f705abf86997", "AQAAAAEAACcQAAAAENoqTLABGTGSfphV5Bi1a9FfD6MW11tUl64Mq1lbeWWMK/t8gJ6jkSR6tgL4/42+Ag==", "91e6c22e-5489-4f27-873c-9b0c7e8b7795" });

            migrationBuilder.CreateIndex(
                name: "IX_StockTake_MainDistributorId",
                table: "StockTake",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTake_ProductId",
                table: "StockTake",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTake_RetailAccountManagerId",
                table: "StockTake",
                column: "RetailAccountManagerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTake");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17c326a8-5d52-424d-97d9-d45521e6860c", "AQAAAAEAACcQAAAAEI3dSLpo9Vjp0/pYXfbYNIjJeAEgJdFrIVO/LhN/6t5zYIJ9ns1sb2GTwidvl9/cjw==", "3a496a21-3f22-4308-8b75-4d7d17549b1c" });
        }
    }
}
