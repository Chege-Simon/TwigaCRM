using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class userTownNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Town_TownId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "TownId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1284df4a-1ddf-42db-956d-f483f931ce83", "AQAAAAEAACcQAAAAEDmqiuX4wQVCS0FjGNSZsZII4c7Asn55wJJDBOodIDX810B5JgrBzwmD4CYziOmnUg==", "148ceaf1-7b81-4c87-a150-1533a4dfd52b" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Town_TownId",
                table: "AspNetUsers",
                column: "TownId",
                principalTable: "Town",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Town_TownId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "TownId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8ebf5c73-e28f-4eab-9d3e-3dc340cdc6ff", "AQAAAAEAACcQAAAAENSjtfbAfiZUZ5z4A4PhPnAKai5H3zk7nbQ24o1CMaXWv3rZq6rZ3ebCgdZXxp0ZGw==", "e413d4b2-e4db-4599-a287-6c6880b9f315" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Town_TownId",
                table: "AspNetUsers",
                column: "TownId",
                principalTable: "Town",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
