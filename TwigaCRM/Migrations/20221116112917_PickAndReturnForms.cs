using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class PickAndReturnForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PickAndReturnForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    PickDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VSAId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    FOAstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TLstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FormUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickAndReturnForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickAndReturnForm_AspNetUsers_VSAId",
                        column: x => x.VSAId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PickAndReturnForm_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickAndReturnProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PickAndReturnFormId = table.Column<int>(type: "int", nullable: false),
                    PickedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PickedVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PickedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnedVolume = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnedValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickAndReturnProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickAndReturnProduct_PickAndReturnForm_PickAndReturnFormId",
                        column: x => x.PickAndReturnFormId,
                        principalTable: "PickAndReturnForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PickAndReturnProduct_Product_ProductId",
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
                values: new object[] { "24384ad5-c432-4360-a562-019f10732271", "AQAAAAEAACcQAAAAECWWO8QvjDsDZUGS34er6gYG8cwtq5yNGCh7u+Rp76oY6Ax/3TNT14rBZIynarQgwQ==", "d4099bc5-5f7b-4cca-9303-09304605d3ac" });

            migrationBuilder.CreateIndex(
                name: "IX_PickAndReturnForm_MainDistributorId",
                table: "PickAndReturnForm",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_PickAndReturnForm_VSAId",
                table: "PickAndReturnForm",
                column: "VSAId");

            migrationBuilder.CreateIndex(
                name: "IX_PickAndReturnProduct_PickAndReturnFormId",
                table: "PickAndReturnProduct",
                column: "PickAndReturnFormId");

            migrationBuilder.CreateIndex(
                name: "IX_PickAndReturnProduct_ProductId",
                table: "PickAndReturnProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PickAndReturnProduct");

            migrationBuilder.DropTable(
                name: "PickAndReturnForm");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f4445805-b4a1-4470-94f7-1f12ac6ed936", "AQAAAAEAACcQAAAAECipSdW/3ONVMXEKZ8Hf5B6l55gRFR+08nvRfbigdpA3uE9wGWzVjgo6jzu9318CBg==", "25263985-1cbd-4054-ac47-a6e98d5adc78" });
        }
    }
}
