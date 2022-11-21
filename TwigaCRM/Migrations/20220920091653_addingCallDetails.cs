using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingCallDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactCategory",
                table: "Call",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactType",
                table: "Call",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a1c089ca-b850-4737-88b4-50e263edd52f", "AQAAAAEAACcQAAAAEKcCo0MiQff8KnUILbbtE2r3MiRg7I4DWzx0FJvVT7NH3h23yC5O0aWNgReuyqrxFA==", "d968b2b6-2b8d-47f3-b856-f4814c4e8708" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactCategory",
                table: "Call");

            migrationBuilder.DropColumn(
                name: "ContactType",
                table: "Call");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4218ceb1-5f40-40ce-a287-317ca153794d", "AQAAAAEAACcQAAAAEP9xEXx6bx9oqsjit6ZOyhu2Desblfl0cAMvuHeNVgWZxrN5m4WwkqUglbK8c5X50g==", "493f781e-6bdd-4e0b-8dfa-e34169eaf65d" });
        }
    }
}
