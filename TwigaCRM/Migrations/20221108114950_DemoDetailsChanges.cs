using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class DemoDetailsChanges : Migration
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
                values: new object[] { "278bdf78-ca89-4f09-9360-46d75146f956", "AQAAAAEAACcQAAAAEM6vCE6ROWNKYDHUwRser8NiWIE5IqM2ner1Xb0dg6LmIItLFlQxQVkZOx5PusuUDw==", "e8907533-8e2d-4de7-8222-7f558b25a547" });

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
                values: new object[] { "63e08e6c-4f51-497d-81fc-2df2fbb2c179", "AQAAAAEAACcQAAAAEGd8x4ijWoCksBMU1UtLWlpQAMLJdZYD0mCzlaB4ASWi/4aomvgPq/av14GbcQn3wQ==", "499fb007-3199-48b2-939f-af917d7701c5" });
        }
    }
}
