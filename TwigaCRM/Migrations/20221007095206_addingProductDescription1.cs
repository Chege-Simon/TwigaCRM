using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingProductDescription1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c94a930f-fcfd-41cd-b547-2802e7557504", "AQAAAAEAACcQAAAAEMu5t5cJ3qcBVOYkqtwSCEDFXCMh6GNPanWMY3ZUzbsFt+eZQ7lu+YEZQN/JsSqCrQ==", "a52b4342-c977-4514-af20-097274abbab9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b01c7b0c-9d1f-4243-9927-391c702aaa16", "AQAAAAEAACcQAAAAEImQj1S3zOZ9WmlOh+qnZ6zJ5Me/41WeLF+GM2q36TcMy+6jfQGn8KE6Pl2BqufqPQ==", "3dba7c53-fe83-4cb7-9697-aeda2dc6b0ee" });
        }
    }
}
