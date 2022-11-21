using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class correctingQuestionResponses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31afbee7-9ee2-4be0-a78c-113e49b455c5", "AQAAAAEAACcQAAAAEPrk07MV2C0VkGADDFJD9nJdB8c/ylVLhJcWy04S6TVPRf7m4dhZYp9vaKbqCFEvUw==", "24831a41-b0ce-46e3-b570-84bb0c140151" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1c089ca-b850-4737-88b4-50e263edd52f", "AQAAAAEAACcQAAAAEKcCo0MiQff8KnUILbbtE2r3MiRg7I4DWzx0FJvVT7NH3h23yC5O0aWNgReuyqrxFA==", "d968b2b6-2b8d-47f3-b856-f4814c4e8708" });
        }
    }
}
