using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingExpenseReceipts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFOAApproved",
                table: "Campaign");

            migrationBuilder.RenameColumn(
                name: "IsHRBApproved",
                table: "Campaign",
                newName: "IsSubmitted");

            migrationBuilder.AddColumn<string>(
                name: "FOAstatus",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HRBstatus",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Campaign",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseReceipt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedExpenseId = table.Column<int>(type: "int", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseReceipt_RequestedExpense_RequestedExpenseId",
                        column: x => x.RequestedExpenseId,
                        principalTable: "RequestedExpense",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c899df0f-4269-4baa-aef2-74c359ee9ede", "AQAAAAEAACcQAAAAEA52/s4uJAB1RHJ7B/G+i8nNU3yO/RPePazWE/JT+lnhZ4B42f67dpgWEzbh3par7A==", "f5aae94e-75c7-4a54-a2e3-58cf69e03fd0" });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseReceipt_RequestedExpenseId",
                table: "ExpenseReceipt",
                column: "RequestedExpenseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseReceipt");

            migrationBuilder.DropColumn(
                name: "FOAstatus",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "HRBstatus",
                table: "Campaign");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Campaign");

            migrationBuilder.RenameColumn(
                name: "IsSubmitted",
                table: "Campaign",
                newName: "IsHRBApproved");

            migrationBuilder.AddColumn<bool>(
                name: "IsFOAApproved",
                table: "Campaign",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3bf97202-6f8b-45db-ad56-b20fdcb807fd", "AQAAAAEAACcQAAAAEEGDXgvTB3J0qLFzaIgZ38MHlDcGhhHw6rmXdi4z+bUmlkOuyerfA4ewney3mnxhgg==", "91ac6ed0-245e-41ba-a01b-89c86b4a4cde" });
        }
    }
}
