using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingCompetetionIntelligence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeInDeployment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Launches = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingFarmers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingStockists = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaPresence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockOuts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RebatsAndSchemes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MajorPurchaseFromCompetition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionActivity_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e5e52fd-8edc-4b8e-961f-7db3ca7ba1f5", "AQAAAAEAACcQAAAAELwmFw61deX9Jq+vx+feCUvgxKHY8N4pR0eDvE5YQKAOnT9LBLVXagYUoiJ+mV32iA==", "e10fb8a1-85d1-45ea-9270-46f1c2161e99" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionActivity_AppUserId",
                table: "CompetitionActivity",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionActivity");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ea2c2ddd-7adf-4215-955c-004bea82d9a3", "AQAAAAEAACcQAAAAEOjLPHtYLa58hBrrTg3TqgPTC9IUEIwGzUiMejScEvCdWTHKGP5ZLRc+gtJK+05DiQ==", "27f3d274-bda4-48f7-9ddf-32ee037d49c3" });
        }
    }
}
