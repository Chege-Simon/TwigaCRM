using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingCampaigns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampaignBudget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    TownId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfCampaigns = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignBudget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampaignBudget_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignBudget_Town_TownId",
                        column: x => x.TownId,
                        principalTable: "Town",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategory",
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
                    table.PrimaryKey("PK_ExpenseCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesPersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfFarmers = table.Column<int>(type: "int", nullable: false),
                    NumberOfPromoters = table.Column<int>(type: "int", nullable: false),
                    CampaignType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsBudgeted = table.Column<bool>(type: "bit", nullable: false),
                    CampaignBudgetId = table.Column<int>(type: "int", nullable: true),
                    IsFOAApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsHRBApproved = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campaign_AspNetUsers_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Campaign_CampaignBudget_CampaignBudgetId",
                        column: x => x.CampaignBudgetId,
                        principalTable: "CampaignBudget",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestedCampaignItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignItemId = table.Column<int>(type: "int", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    RequestedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOAApprovedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FOAApprovedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedCampaignItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedCampaignItem_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedCampaignItem_CampaignItem_CampaignItemId",
                        column: x => x.CampaignItemId,
                        principalTable: "CampaignItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestedExpense",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    ExpenseCategoryId = table.Column<int>(type: "int", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFOAApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedExpense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedExpense_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedExpense_ExpenseCategory_ExpenseCategoryId",
                        column: x => x.ExpenseCategoryId,
                        principalTable: "ExpenseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestedProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CurrentMovement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMovementValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedMovement = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedMovementValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImportantObservation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpAction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedProduct_Campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestedProduct_Product_ProductId",
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
                values: new object[] { "3bf97202-6f8b-45db-ad56-b20fdcb807fd", "AQAAAAEAACcQAAAAEEGDXgvTB3J0qLFzaIgZ38MHlDcGhhHw6rmXdi4z+bUmlkOuyerfA4ewney3mnxhgg==", "91ac6ed0-245e-41ba-a01b-89c86b4a4cde" });

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_CampaignBudgetId",
                table: "Campaign",
                column: "CampaignBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaign_SalesPersonId",
                table: "Campaign",
                column: "SalesPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignBudget_FinancialYearId",
                table: "CampaignBudget",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_CampaignBudget_TownId",
                table: "CampaignBudget",
                column: "TownId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCampaignItem_CampaignId",
                table: "RequestedCampaignItem",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedCampaignItem_CampaignItemId",
                table: "RequestedCampaignItem",
                column: "CampaignItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedExpense_CampaignId",
                table: "RequestedExpense",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedExpense_ExpenseCategoryId",
                table: "RequestedExpense",
                column: "ExpenseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedProduct_CampaignId",
                table: "RequestedProduct",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedProduct_ProductId",
                table: "RequestedProduct",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestedCampaignItem");

            migrationBuilder.DropTable(
                name: "RequestedExpense");

            migrationBuilder.DropTable(
                name: "RequestedProduct");

            migrationBuilder.DropTable(
                name: "CampaignItem");

            migrationBuilder.DropTable(
                name: "ExpenseCategory");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DropTable(
                name: "CampaignBudget");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1cb5007a-b391-4a86-99e2-580d1dab6adf", "AQAAAAEAACcQAAAAEN9FATMpMy2eo4EPmuy0V/9W0snJvf+zowM0DFadlOCIDbFIUS1SL1//IeR9fj27kQ==", "16ad548d-62d2-475b-8041-5e580903c1c1" });
        }
    }
}
