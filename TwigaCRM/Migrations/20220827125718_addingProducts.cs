using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CropAndAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropAndAnimal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropAndAnimal_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PestAndDisease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PestAndDisease", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackagingSize = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BusinessLineId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_BusinessLine_BusinessLineId",
                        column: x => x.BusinessLineId,
                        principalTable: "BusinessLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CropAndAnimalPestAndDisease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CropAndAnimalId = table.Column<int>(type: "int", nullable: false),
                    PestAndDiseaseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropAndAnimalPestAndDisease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CropAndAnimalPestAndDisease_CropAndAnimal_CropAndAnimalId",
                        column: x => x.CropAndAnimalId,
                        principalTable: "CropAndAnimal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CropAndAnimalPestAndDisease_PestAndDisease_PestAndDiseaseId",
                        column: x => x.PestAndDiseaseId,
                        principalTable: "PestAndDisease",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCropAndAnimal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CropAndAnimalId = table.Column<int>(type: "int", nullable: false),
                    Servings = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCropAndAnimal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCropAndAnimal_CropAndAnimal_CropAndAnimalId",
                        column: x => x.CropAndAnimalId,
                        principalTable: "CropAndAnimal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCropAndAnimal_Product_ProductId",
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
                values: new object[] { "7a88c97a-d4c3-421d-9bff-5b47d4f77394", "AQAAAAEAACcQAAAAEIbx/rjez99CLYYJIblvhC2ZYhAx4WU6TOLHoUsJHkyoTyqvrpcblYbgr+0QSs+Fqg==", "239c4c83-5630-4e66-aec1-2072108378ea" });

            migrationBuilder.CreateIndex(
                name: "IX_CropAndAnimal_SectorId",
                table: "CropAndAnimal",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CropAndAnimalPestAndDisease_CropAndAnimalId",
                table: "CropAndAnimalPestAndDisease",
                column: "CropAndAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_CropAndAnimalPestAndDisease_PestAndDiseaseId",
                table: "CropAndAnimalPestAndDisease",
                column: "PestAndDiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_BusinessLineId",
                table: "Product",
                column: "BusinessLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCropAndAnimal_CropAndAnimalId",
                table: "ProductCropAndAnimal",
                column: "CropAndAnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCropAndAnimal_ProductId",
                table: "ProductCropAndAnimal",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CropAndAnimalPestAndDisease");

            migrationBuilder.DropTable(
                name: "ProductCropAndAnimal");

            migrationBuilder.DropTable(
                name: "PestAndDisease");

            migrationBuilder.DropTable(
                name: "CropAndAnimal");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "067c0778-eddc-4c62-aa6c-c39c256cf6a4", "AQAAAAEAACcQAAAAEKw75C5KUsbJoeKG0RH9tc1b7DK8oFXPK69Dunl9GL60EXqbKQfibYHFJeCsm1YLzw==", "e981167d-8b0d-4b32-b383-722a1c886a92" });
        }
    }
}
