using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class nullableCustomerZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2f037b44-08cb-47c8-9b03-22a60678ad8d", "AQAAAAEAACcQAAAAEH1Enp1RiHRl8Oky2s02epPO/70vTdJ51WrR6vAuq5n1AOMAKpjnxIfxIsUIMWiGTA==", "dbdafd14-fc61-47a2-a8fe-ec1d222fd33e" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2dedac96-ad25-4b47-ada4-55aa113d1d83", "AQAAAAEAACcQAAAAEEbQF3THS7TMAD/LohuUh4o0TNwNGcRk1uVPoz0izr0JiHBnzFJ48MP7pVzyDtqvLw==", "a09bbf6f-6e75-475e-96f4-68568afa6ab8" });
        }
    }
}
