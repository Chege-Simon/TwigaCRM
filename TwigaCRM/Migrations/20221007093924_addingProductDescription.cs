using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingProductDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b01c7b0c-9d1f-4243-9927-391c702aaa16", "AQAAAAEAACcQAAAAEImQj1S3zOZ9WmlOh+qnZ6zJ5Me/41WeLF+GM2q36TcMy+6jfQGn8KE6Pl2BqufqPQ==", "3dba7c53-fe83-4cb7-9697-aeda2dc6b0ee" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9bea3cdc-099b-47cd-8b5f-a5113620de4f", "AQAAAAEAACcQAAAAEIBn9LtrxUR3Wj2Hab530AEPNMBOC9/piUIWZXqhB9H1um4fNNZg0zgYT++3do0haQ==", "68a8bd3a-ecac-4db2-9987-9744dc6c7871" });
        }
    }
}
