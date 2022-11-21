using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingRAM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RAMCollectionTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMCollectionTarget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMCollectionTarget_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RAMCollectionTarget_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAMDailyCollectionReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMDailyCollectionReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMDailyCollectionReport_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RAMDailySaleReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMDailySaleReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMDailySaleReport_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RAMFieldReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerRelatedIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeatherAndCropSituationUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductRelatedIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Opportunities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMFieldReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMFieldReport_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RAMPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMPlan_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RAMSaleTarget",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FinancialYearId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMSaleTarget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMSaleTarget_AspNetUsers_RAMId",
                        column: x => x.RAMId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RAMSaleTarget_FinancialYear_FinancialYearId",
                        column: x => x.FinancialYearId,
                        principalTable: "FinancialYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAMCollectionTargetMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    ExpectedCollection = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAMCollectionTargetId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMCollectionTargetMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMCollectionTargetMapping_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMCollectionTargetMapping_RAMCollectionTarget_RAMCollectionTargetId",
                        column: x => x.RAMCollectionTargetId,
                        principalTable: "RAMCollectionTarget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAMDailyCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    DailyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAMDailyCollectionReportId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMDailyCollection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMDailyCollection_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMDailyCollection_RAMDailyCollectionReport_RAMDailyCollectionReportId",
                        column: x => x.RAMDailyCollectionReportId,
                        principalTable: "RAMDailyCollectionReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAMDailySale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    DailyAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAMDailySaleReportId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMDailySale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMDailySale_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMDailySale_RAMDailySaleReport_RAMDailySaleReportId",
                        column: x => x.RAMDailySaleReportId,
                        principalTable: "RAMDailySaleReport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RAMRoute",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RAMPlanId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ZoneId = table.Column<int>(type: "int", nullable: true),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualLong = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualLat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMRoute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMRoute_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMRoute_RAMPlan_RAMPlanId",
                        column: x => x.RAMPlanId,
                        principalTable: "RAMPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMRoute_Zone_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zone",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RAMSaleTargetMapping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainDistributorId = table.Column<int>(type: "int", nullable: false),
                    ExpectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RAMSaleTargetId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RAMSaleTargetMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RAMSaleTargetMapping_Customer_MainDistributorId",
                        column: x => x.MainDistributorId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RAMSaleTargetMapping_RAMSaleTarget_RAMSaleTargetId",
                        column: x => x.RAMSaleTargetId,
                        principalTable: "RAMSaleTarget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76acd23e-7b31-4f14-9ae2-cdf180c3129c", "AQAAAAEAACcQAAAAEDmpRvGZfIFmRBoJpbU6Y532P83xkxAjX/uYUa1Aaze0ycN0610vfHD+63PYHRHMmQ==", "ab0d0374-bcbf-4a79-9600-99c7e25c1d2b" });

            migrationBuilder.CreateIndex(
                name: "IX_RAMCollectionTarget_FinancialYearId",
                table: "RAMCollectionTarget",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMCollectionTarget_RAMId",
                table: "RAMCollectionTarget",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMCollectionTargetMapping_MainDistributorId",
                table: "RAMCollectionTargetMapping",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMCollectionTargetMapping_RAMCollectionTargetId",
                table: "RAMCollectionTargetMapping",
                column: "RAMCollectionTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailyCollection_MainDistributorId",
                table: "RAMDailyCollection",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailyCollection_RAMDailyCollectionReportId",
                table: "RAMDailyCollection",
                column: "RAMDailyCollectionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailyCollectionReport_RAMId",
                table: "RAMDailyCollectionReport",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailySale_MainDistributorId",
                table: "RAMDailySale",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailySale_RAMDailySaleReportId",
                table: "RAMDailySale",
                column: "RAMDailySaleReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMDailySaleReport_RAMId",
                table: "RAMDailySaleReport",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMFieldReport_RAMId",
                table: "RAMFieldReport",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMPlan_RAMId",
                table: "RAMPlan",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMRoute_MainDistributorId",
                table: "RAMRoute",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMRoute_RAMPlanId",
                table: "RAMRoute",
                column: "RAMPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMRoute_ZoneId",
                table: "RAMRoute",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMSaleTarget_FinancialYearId",
                table: "RAMSaleTarget",
                column: "FinancialYearId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMSaleTarget_RAMId",
                table: "RAMSaleTarget",
                column: "RAMId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMSaleTargetMapping_MainDistributorId",
                table: "RAMSaleTargetMapping",
                column: "MainDistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_RAMSaleTargetMapping_RAMSaleTargetId",
                table: "RAMSaleTargetMapping",
                column: "RAMSaleTargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RAMCollectionTargetMapping");

            migrationBuilder.DropTable(
                name: "RAMDailyCollection");

            migrationBuilder.DropTable(
                name: "RAMDailySale");

            migrationBuilder.DropTable(
                name: "RAMFieldReport");

            migrationBuilder.DropTable(
                name: "RAMRoute");

            migrationBuilder.DropTable(
                name: "RAMSaleTargetMapping");

            migrationBuilder.DropTable(
                name: "RAMCollectionTarget");

            migrationBuilder.DropTable(
                name: "RAMDailyCollectionReport");

            migrationBuilder.DropTable(
                name: "RAMDailySaleReport");

            migrationBuilder.DropTable(
                name: "RAMPlan");

            migrationBuilder.DropTable(
                name: "RAMSaleTarget");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f84f54e-ef20-48aa-9ad4-dc26e80c268f", "AQAAAAEAACcQAAAAECMLNBTJrgBdrpiefnmciOz7OfwNo3Ioima9L82u+0O5dF21NqkKvJJW0V7Ne1zsXw==", "b56cd9ce-cccc-4b5c-ac80-e3c4aeb053ad" });
        }
    }
}
