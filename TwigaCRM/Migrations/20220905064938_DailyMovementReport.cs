using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class DailyMovementReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMovement_AspNetUsers_SalesPersonId",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "FOAstatus",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "HRBstatus",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "SalesDate",
                table: "DailyMovement");

            migrationBuilder.RenameColumn(
                name: "SalesPersonId",
                table: "DailyMovement",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMovement_SalesPersonId",
                table: "DailyMovement",
                newName: "IX_DailyMovement_AppUserId");

            migrationBuilder.AddColumn<int>(
                name: "DailyMovementReportId",
                table: "DailyMovement",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DailyMovementReport",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesPersonId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    FOAstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HRBstatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyMovementReport", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyMovementReport_AspNetUsers_SalesPersonId",
                        column: x => x.SalesPersonId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b091c41c-1b55-4ef9-8dc5-8f76897cc620", "AQAAAAEAACcQAAAAENUeciaCkDlTDcevdx5mxiAEoz1FD/TMifNybV7fPhxNg31qpohiIFRo52tQlfVTiQ==", "de92d7d9-3aab-4f15-9bd2-fbae8666e404" });

            migrationBuilder.CreateIndex(
                name: "IX_DailyMovement_DailyMovementReportId",
                table: "DailyMovement",
                column: "DailyMovementReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyMovementReport_SalesPersonId",
                table: "DailyMovementReport",
                column: "SalesPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMovement_AspNetUsers_AppUserId",
                table: "DailyMovement",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMovement_DailyMovementReport_DailyMovementReportId",
                table: "DailyMovement",
                column: "DailyMovementReportId",
                principalTable: "DailyMovementReport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMovement_AspNetUsers_AppUserId",
                table: "DailyMovement");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyMovement_DailyMovementReport_DailyMovementReportId",
                table: "DailyMovement");

            migrationBuilder.DropTable(
                name: "DailyMovementReport");

            migrationBuilder.DropIndex(
                name: "IX_DailyMovement_DailyMovementReportId",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "DailyMovementReportId",
                table: "DailyMovement");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "DailyMovement",
                newName: "SalesPersonId");

            migrationBuilder.RenameIndex(
                name: "IX_DailyMovement_AppUserId",
                table: "DailyMovement",
                newName: "IX_DailyMovement_SalesPersonId");

            migrationBuilder.AddColumn<string>(
                name: "FOAstatus",
                table: "DailyMovement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HRBstatus",
                table: "DailyMovement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "DailyMovement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SalesDate",
                table: "DailyMovement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d9e2824-deb7-4cb6-8faf-2cd0cfe44578", "AQAAAAEAACcQAAAAEBGYQG4vbk/2wX/2a8dWstm7mj6eGzpIgXgIf0ROPTYSdHpDdiqMNFnVeIe2WsixTA==", "008d0230-f020-4894-8fda-3fd199d228ce" });

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMovement_AspNetUsers_SalesPersonId",
                table: "DailyMovement",
                column: "SalesPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
