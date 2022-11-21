using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingSaleMovementCreatorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "SalesMovement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0be45bfe-01c5-4e84-8dea-03040776eb2f", "AQAAAAEAACcQAAAAEJozzysp8mJeU9qIMxhhf1/ne6U4nGOHxiB5FzH6+S61uSE7xRFxq66iyEQi2GQhyA==", "04ffcd31-960b-415f-ac5c-cb2a07438230" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "SalesMovement");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76acd23e-7b31-4f14-9ae2-cdf180c3129c", "AQAAAAEAACcQAAAAEDmpRvGZfIFmRBoJpbU6Y532P83xkxAjX/uYUa1Aaze0ycN0610vfHD+63PYHRHMmQ==", "ab0d0374-bcbf-4a79-9600-99c7e25c1d2b" });
        }
    }
}
