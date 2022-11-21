using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class errorCorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "90e51269-ba4c-4e1b-a12f-2045cd1d6daa", "AQAAAAEAACcQAAAAEJ9rIyTL2+0h7Css9laL2rXT8pqdRfaWvRKO9yVneowSFXCh8wJll8fc7cEjq4KwfA==", "9078e92c-d7a3-4e41-811c-d595521236a3" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0e903146-0481-458f-9d04-e6e440bf810f", "AQAAAAEAACcQAAAAEKFqYfcu/31hDMjap4TxDGFVVo5jSHreh51AIdyr6vDSFDV5BGFuFrkBMp1T9TnUbw==", "214ee4ca-b800-4bc0-b045-4adfa077b6a3" });
        }
    }
}
