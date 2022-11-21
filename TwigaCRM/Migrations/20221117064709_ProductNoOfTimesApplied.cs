using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class ProductNoOfTimesApplied : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "NoOfTimeApplied",
                table: "ProductCropAndAnimal",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "893bbf51-eb2d-4f50-a293-cc00375928c4", "AQAAAAEAACcQAAAAEAo6XYQxufIXu2DhLs7G45NSvOnxFYWula4RsTcrTrxodCCtE5L1h59vbef2i1WicQ==", "8a4df928-78c1-4b33-90e9-f5a6ed77f50b" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfTimeApplied",
                table: "ProductCropAndAnimal");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "24384ad5-c432-4360-a562-019f10732271", "AQAAAAEAACcQAAAAECWWO8QvjDsDZUGS34er6gYG8cwtq5yNGCh7u+Rp76oY6Ax/3TNT14rBZIynarQgwQ==", "d4099bc5-5f7b-4cca-9303-09304605d3ac" });
        }
    }
}
