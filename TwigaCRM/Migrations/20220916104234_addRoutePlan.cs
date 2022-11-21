using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addRoutePlan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e2b1796a-6a02-469c-a402-fc2fcd07d521", "AQAAAAEAACcQAAAAEOX5gF1Jc7VF94lsBNUzPlDTVvUHAiGVDJKmDLlwObbYJTPgQkvPlSKWuxpz/pXkbA==", "0e6dc287-3816-4ce0-8318-cd62e811af44" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b5fe4512-5ae4-4694-a063-117138c191a5", "AQAAAAEAACcQAAAAEE1DMf/91fLCS1PkeW1dVu4OLYG97Xtklb/8AcQnbofcSi0o4kanN3rRmB0dTglcPg==", "56f8b32e-10b9-443a-bf0c-8825608af1f4" });
        }
    }
}
