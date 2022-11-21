using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class correctingQuestionResponses1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc271c6a-3f5b-424a-8dc5-5b749b3fa3b4", "AQAAAAEAACcQAAAAEEK2J7Nnw0ECiYQ7us9UnQh32KM5JfZ8Uo+0G3XfI+vhBzlYQ6GvuAupMi1a9DptTA==", "3f2bffd1-d0b8-44d8-b3a4-5042e41b3d8c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "31afbee7-9ee2-4be0-a78c-113e49b455c5", "AQAAAAEAACcQAAAAEPrk07MV2C0VkGADDFJD9nJdB8c/ylVLhJcWy04S6TVPRf7m4dhZYp9vaKbqCFEvUw==", "24831a41-b0ce-46e3-b570-84bb0c140151" });
        }
    }
}
