using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class removeUserZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Zone_ZoneId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ZoneId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "17c326a8-5d52-424d-97d9-d45521e6860c", "AQAAAAEAACcQAAAAEI3dSLpo9Vjp0/pYXfbYNIjJeAEgJdFrIVO/LhN/6t5zYIJ9ns1sb2GTwidvl9/cjw==", "3a496a21-3f22-4308-8b75-4d7d17549b1c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1284df4a-1ddf-42db-956d-f483f931ce83", "AQAAAAEAACcQAAAAEDmqiuX4wQVCS0FjGNSZsZII4c7Asn55wJJDBOodIDX810B5JgrBzwmD4CYziOmnUg==", "148ceaf1-7b81-4c87-a150-1533a4dfd52b" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ZoneId",
                table: "AspNetUsers",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Zone_ZoneId",
                table: "AspNetUsers",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");
        }
    }
}
