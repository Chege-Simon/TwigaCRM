using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingDailyMovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DailyMovement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    StockistOrFarmerId = table.Column<int>(type: "int", nullable: false),
                    SalesPersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedByFOA = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedByHRB = table.Column<bool>(type: "bit", nullable: false),
                    RejectedByFOA = table.Column<bool>(type: "bit", nullable: false),
                    RejectedByHRB = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyMovement_AspNetUsers_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyMovement_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyMovement_Product_ProductId",
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
                values: new object[] { "5cc95bd9-eef8-4099-8aaf-1e4d74a63e15", "AQAAAAEAACcQAAAAEAu0O2HXmzzI/8iXp5JXUDJO0rls1+g+jPRGXueFM+J0xCLEkf0AeUYICjNISq5Cqg==", "feaee6b5-c88a-41af-b2b0-9d7ba9064e88" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMovement_MainDistributorId",
                table: "DailyMovement",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMovement_ProductId",
                table: "DailyMovement",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMovement_SalesPersonId",
                table: "DailyMovement",
                column: "SalesPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyMovement");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f25ac3e2-8ad1-4cc9-afc5-be121ee352ce", "AQAAAAEAACcQAAAAEEJhBKQ8dg0LaSyfPPhW1/0pLMSE1DNz2cM6adYM0/I1prgVTrgD2Ih2GbpTdIp5rA==", "599e7489-da7e-48ef-bdd9-2c9c258019da" });
        }
    }
}
