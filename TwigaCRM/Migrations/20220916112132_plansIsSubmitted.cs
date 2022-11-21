using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class plansIsSubmitted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "Plan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8e746d81-35c1-4cf2-b70a-624fde805713", "AQAAAAEAACcQAAAAEEcHdPHccZfYeTO6yNpqwaOAJabFg20cPWOdqcDbPkbhhd5Zw6POPivZeRud1SKnTw==", "7eff6481-7efe-4725-a708-23b8799d39f2" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "Plan");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "64469e93-2133-4928-aacc-de4786e32579", "AQAAAAEAACcQAAAAEIJu5uE4nldGvLpkkw1eGZcDKd2VHp4MYlpNCQhIU8CInIGLPikjQP5P3J/L03NyMw==", "6612f3dc-a83c-4866-8977-8d283b5f1f16" });
        }
    }
}
