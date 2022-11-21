using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingBusinessLineAndSector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessLine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sector", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserBusinessLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BusinessLineId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBusinessLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserBusinessLine_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserBusinessLine_BusinessLine_BusinessLineId",
                        column: x => x.BusinessLineId,
                        principalTable: "BusinessLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSector_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSector_Sector_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "16c4bbe0-9c76-457f-a66d-42b92bc1390e", "AQAAAAEAACcQAAAAELxbk4AoaVvaPOWNLMoZbEGwOG3yzZhZINryFG1O6YRBBXyWbBT8H8rkSzwCMuXJGw==", "3fb4f3cf-c791-4d52-8e74-6f5d83bb89ae" });

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessLine_AppUserId",
                table: "UserBusinessLine",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBusinessLine_BusinessLineId",
                table: "UserBusinessLine",
                column: "BusinessLineId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSector_AppUserId",
                table: "UserSector",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSector_SectorId",
                table: "UserSector",
                column: "SectorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBusinessLine");

            migrationBuilder.DropTable(
                name: "UserSector");

            migrationBuilder.DropTable(
                name: "BusinessLine");

            migrationBuilder.DropTable(
                name: "Sector");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "db76eaae-a91c-4cef-8c29-4281ef39a2e7", "AQAAAAEAACcQAAAAEPVWazpXJNv81x659QgYFP7FeeRDwKXihYg8hvpNFexhXZqa4m2UGkQLpzzzxcfy9A==", "37050073-b4b6-47e1-936b-89ccd0bf1ef6" });
        }
    }
}
