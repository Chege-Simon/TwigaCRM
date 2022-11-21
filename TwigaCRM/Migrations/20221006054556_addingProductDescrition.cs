using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingProductDescrition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Product",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9bea3cdc-099b-47cd-8b5f-a5113620de4f", "AQAAAAEAACcQAAAAEIBn9LtrxUR3Wj2Hab530AEPNMBOC9/piUIWZXqhB9H1um4fNNZg0zgYT++3do0haQ==", "68a8bd3a-ecac-4db2-9987-9744dc6c7871" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Product");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "08052742-dd14-4fee-ada2-6ec7cd47ba57", "AQAAAAEAACcQAAAAECsv9QGqGfLU7J1/pDZtKJ7FYvBj1f+JsjpkSHqMjSO0lFLGxYKqZQuOF3L5iessYg==", "26aee2af-b2bc-4613-92d7-a2b8385a5d18" });
        }
    }
}
