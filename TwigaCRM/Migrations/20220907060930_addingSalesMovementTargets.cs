using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingSalesMovementTargets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalesMovement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    SalesPersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    HRBstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesMovement_AspNetUsers_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SalesMovement_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesMovementId = table.Column<int>(type: "int", nullable: false),
                    CropAndAnimalId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CropAndAnimalQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BusinessPotential = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarketShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Target_CropAndAnimal_CropAndAnimalId",
                        column: x => x.CropAndAnimalId,
                        principalTable: "CropAndAnimal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Target_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Target_SalesMovement_SalesMovementId",
                        column: x => x.SalesMovementId,
                        principalTable: "SalesMovement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1cb5007a-b391-4a86-99e2-580d1dab6adf", "AQAAAAEAACcQAAAAEN9FATMpMy2eo4EPmuy0V/9W0snJvf+zowM0DFadlOCIDbFIUS1SL1//IeR9fj27kQ==", "16ad548d-62d2-475b-8041-5e580903c1c1" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesMovement_FinancialYearId",
                table: "SalesMovement",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesMovement_SalesPersonId",
                table: "SalesMovement",
                column: "SalesPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_CropAndAnimalId",
                table: "Target",
                column: "CropAndAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_ProductId",
                table: "Target",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Target_SalesMovementId",
                table: "Target",
                column: "SalesMovementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Target");

            migrationBuilder.DropTable(
                name: "SalesMovement");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "943abed6-cc90-4e9b-8acb-150c1d396989", "AQAAAAEAACcQAAAAEIBvR/APj2G6ZCcwNZ1OkVTG7jjmSqPrKq++a6ISclpZ5XLM4cI6nucduBXY5Cjusw==", "94b46e9c-b00e-4f5f-a01e-43b27a4a4a36" });
        }
    }
}
