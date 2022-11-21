using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class DemoChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "DemoDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DemoProductRequisition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemoId = table.Column<int>(type: "int", nullable: false),
                    StockistId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quanitity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InvoiceUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProductRequisition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoProductRequisition_Customer_StockistId",
                        column: x => x.StockistId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemoProductRequisition_Demo_DemoId",
                        column: x => x.DemoId,
                        principalTable: "Demo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemoProductRequisition_Product_ProductId",
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
                values: new object[] { "cb051508-3b49-4534-a120-3e46569f28e5", "AQAAAAEAACcQAAAAEJYcQgUq88HuDj7xNVzeqPQoNvHTLY+odx7zwoGyukELaJHiGTqSMnQQdKtqPKRe0g==", "1d02de74-fbdd-43d7-8128-6bc0312cf43b" });

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_SiteId",
                table: "DemoDetail",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProductRequisition_DemoId",
                table: "DemoProductRequisition",
                column: "DemoId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProductRequisition_ProductId",
                table: "DemoProductRequisition",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProductRequisition_StockistId",
                table: "DemoProductRequisition",
                column: "StockistId");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoDetail_Customer_SiteId",
                table: "DemoDetail",
                column: "SiteId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoDetail_Customer_SiteId",
                table: "DemoDetail");

            migrationBuilder.DropTable(
                name: "DemoProductRequisition");

            migrationBuilder.DropIndex(
                name: "IX_DemoDetail_SiteId",
                table: "DemoDetail");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "DemoDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "28efb83c-175c-478a-9afc-02f6c2fbb4c4", "AQAAAAEAACcQAAAAEEzY/J6UZFPEeDnWCpHz6IE/aB7UNheXunovULnSU8Ss+2xjF2qnVayUUZCwWn4CYg==", "955bfde3-a344-4be1-ba77-37d6c49478ce" });
        }
    }
}
