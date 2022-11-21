using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingFarmSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FarmSize",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2dedac96-ad25-4b47-ada4-55aa113d1d83", "AQAAAAEAACcQAAAAEEbQF3THS7TMAD/LohuUh4o0TNwNGcRk1uVPoz0izr0JiHBnzFJ48MP7pVzyDtqvLw==", "a09bbf6f-6e75-475e-96f4-68568afa6ab8" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FarmSize",
                table: "Customer");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60a195c3-d6fd-483f-a823-193c9c2796ac", "AQAAAAEAACcQAAAAEAos/lCSrf5OpiMyep9+2WL3KHd5tic9vM4OEvUeSvn7LaiFiyG2VaDTileRrO0SFQ==", "90ec2add-58e6-4c38-8435-55cf1b15c206" });
        }
    }
}
