using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingDemo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetingProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetingProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Demo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalesPersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DemoType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    FOAstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PDstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Demo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Demo_AspNetUsers_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductCompetingProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompetingProductId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCompetingProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCompetingProduct_CompetingProduct_CompetingProductId",
                        column: x => x.CompetingProductId,
                        principalTable: "CompetingProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCompetingProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DemoId = table.Column<int>(type: "int", nullable: false),
                    CropOrAnimalId = table.Column<int>(type: "int", nullable: false),
                    CropAndAnimalId = table.Column<int>(type: "int", nullable: true),
                    TargetPestOrDiseaseId = table.Column<int>(type: "int", nullable: false),
                    PestAndDiseaseId = table.Column<int>(type: "int", nullable: true),
                    RequestedQuantityOfSample = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestedNumberOfDemos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovedQuantityOfSample = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovedNumberOfDemos = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompetingProductId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFOAApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsPDApproved = table.Column<bool>(type: "bit", nullable: false),
                    CropAndAnimalPestAndDiseaseId = table.Column<int>(type: "int", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoDetail_CompetingProduct_CompetingProductId",
                        column: x => x.CompetingProductId,
                        principalTable: "CompetingProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemoDetail_CropAndAnimal_CropAndAnimalId",
                        column: x => x.CropAndAnimalId,
                        principalTable: "CropAndAnimal",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemoDetail_CropAndAnimalPestAndDisease_CropAndAnimalPestAndDiseaseId",
                        column: x => x.CropAndAnimalPestAndDiseaseId,
                        principalTable: "CropAndAnimalPestAndDisease",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemoDetail_Demo_DemoId",
                        column: x => x.DemoId,
                        principalTable: "Demo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DemoDetail_PestAndDisease_PestAndDiseaseId",
                        column: x => x.PestAndDiseaseId,
                        principalTable: "PestAndDisease",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DemoDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DemoProgressReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinalReport = table.Column<bool>(type: "bit", nullable: false),
                    DemoDetailId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemoProgressReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DemoProgressReport_DemoDetail_DemoDetailId",
                        column: x => x.DemoDetailId,
                        principalTable: "DemoDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "221504bd-3905-4c2f-9641-8d93e6fcc462", "AQAAAAEAACcQAAAAEP1z5nVuAdsdbg4QTjVyJ3AgIq6NHDBiubSsc96OoFVl9y+4+Z3lkDislO15OJKBLw==", "ad14e2c7-e959-45e2-907a-ee30af365655" });

            migrationBuilder.CreateIndex(
                name: "IX_Demo_SalesPersonId",
                table: "Demo",
                column: "SalesPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_CompetingProductId",
                table: "DemoDetail",
                column: "CompetingProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_CropAndAnimalId",
                table: "DemoDetail",
                column: "CropAndAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_CropAndAnimalPestAndDiseaseId",
                table: "DemoDetail",
                column: "CropAndAnimalPestAndDiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_DemoId",
                table: "DemoDetail",
                column: "DemoId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_PestAndDiseaseId",
                table: "DemoDetail",
                column: "PestAndDiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoDetail_ProductId",
                table: "DemoDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DemoProgressReport_DemoDetailId",
                table: "DemoProgressReport",
                column: "DemoDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetingProduct_CompetingProductId",
                table: "ProductCompetingProduct",
                column: "CompetingProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetingProduct_ProductId",
                table: "ProductCompetingProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DemoProgressReport");

            migrationBuilder.DropTable(
                name: "ProductCompetingProduct");

            migrationBuilder.DropTable(
                name: "DemoDetail");

            migrationBuilder.DropTable(
                name: "CompetingProduct");

            migrationBuilder.DropTable(
                name: "Demo");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7d47e71b-dd1b-448e-aaf4-f705abf86997", "AQAAAAEAACcQAAAAENoqTLABGTGSfphV5Bi1a9FfD6MW11tUl64Mq1lbeWWMK/t8gJ6jkSR6tgL4/42+Ag==", "91e6c22e-5489-4f27-873c-9b0c7e8b7795" });
        }
    }
}
