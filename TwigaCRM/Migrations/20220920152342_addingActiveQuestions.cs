using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingActiveQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Question",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "60a195c3-d6fd-483f-a823-193c9c2796ac", "AQAAAAEAACcQAAAAEAos/lCSrf5OpiMyep9+2WL3KHd5tic9vM4OEvUeSvn7LaiFiyG2VaDTileRrO0SFQ==", "90ec2add-58e6-4c38-8435-55cf1b15c206" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Question");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc271c6a-3f5b-424a-8dc5-5b749b3fa3b4", "AQAAAAEAACcQAAAAEEK2J7Nnw0ECiYQ7us9UnQh32KM5JfZ8Uo+0G3XfI+vhBzlYQ6GvuAupMi1a9DptTA==", "3f2bffd1-d0b8-44d8-b3a4-5042e41b3d8c" });
        }
    }
}
