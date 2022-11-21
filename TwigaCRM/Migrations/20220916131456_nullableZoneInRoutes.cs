using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class nullableZoneInRoutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Route_Zone_ZoneId",
                table: "Route");

            migrationBuilder.AlterColumn<int>(
                name: "ZoneId",
                table: "Route",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsFOAApproved",
                table: "Route",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e903146-0481-458f-9d04-e6e440bf810f", "AQAAAAEAACcQAAAAEKFqYfcu/31hDMjap4TxDGFVVo5jSHreh51AIdyr6vDSFDV5BGFuFrkBMp1T9TnUbw==", "214ee4ca-b800-4bc0-b045-4adfa077b6a3" });

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Zone_ZoneId",
                table: "Route",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Route_Zone_ZoneId",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "IsFOAApproved",
                table: "Route");

            migrationBuilder.AlterColumn<int>(
                name: "ZoneId",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b8602039-bad1-478c-84e9-b381c27e7123", "AQAAAAEAACcQAAAAELLCzqEn4gbXFTFq0v9LR5Mhi5LjQQ1l4kEsfXLaPvJimhcV7e8RduvKNZb4xVyOeA==", "5f7ff6c4-3ffe-4a61-97a7-a05b07552754" });

            migrationBuilder.AddForeignKey(
                name: "FK_Route_Zone_ZoneId",
                table: "Route",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
