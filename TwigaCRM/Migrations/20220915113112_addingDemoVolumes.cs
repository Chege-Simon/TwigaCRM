using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingDemoVolumes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ApprovedVolumeOfProduct",
                table: "DemoDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RequestedVolumeOfProduct",
                table: "DemoDetail",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dcae36b1-1ba8-4d5e-b06e-ff3c8d12eb5d", "AQAAAAEAACcQAAAAELnOvg20cCIqqK+B82SaS3iNAZv3FfpcYHRB9xeRPyfup5WFQRVePyctupN1eRrmTA==", "c42ad473-c04f-4cb4-bf8c-a688f676532e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedVolumeOfProduct",
                table: "DemoDetail");

            migrationBuilder.DropColumn(
                name: "RequestedVolumeOfProduct",
                table: "DemoDetail");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "221504bd-3905-4c2f-9641-8d93e6fcc462", "AQAAAAEAACcQAAAAEP1z5nVuAdsdbg4QTjVyJ3AgIq6NHDBiubSsc96OoFVl9y+4+Z3lkDislO15OJKBLw==", "ad14e2c7-e959-45e2-907a-ee30af365655" });
        }
    }
}
