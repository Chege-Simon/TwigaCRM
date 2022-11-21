using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class dailymovementapprovals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByFOA",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "ApprovedByHRB",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "RejectedByFOA",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "RejectedByHRB",
                table: "DailyMovement");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8d9e2824-deb7-4cb6-8faf-2cd0cfe44578", "AQAAAAEAACcQAAAAEBGYQG4vbk/2wX/2a8dWstm7mj6eGzpIgXgIf0ROPTYSdHpDdiqMNFnVeIe2WsixTA==", "008d0230-f020-4894-8fda-3fd199d228ce" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FOAstatus",
                table: "DailyMovement");

            migrationBuilder.DropColumn(
                name: "HRBstatus",
                table: "DailyMovement");

            migrationBuilder.AddColumn<bool>(
                name: "ApprovedByFOA",
                table: "DailyMovement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ApprovedByHRB",
                table: "DailyMovement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByFOA",
                table: "DailyMovement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RejectedByHRB",
                table: "DailyMovement",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5cc95bd9-eef8-4099-8aaf-1e4d74a63e15", "AQAAAAEAACcQAAAAEAu0O2HXmzzI/8iXp5JXUDJO0rls1+g+jPRGXueFM+J0xCLEkf0AeUYICjNISq5Cqg==", "feaee6b5-c88a-41af-b2b0-9d7ba9064e88" });
        }
    }
}
