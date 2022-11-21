using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class changeHRBtoHRM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HRBstatus",
                table: "SalesMovement",
                newName: "HRMstatus");

            migrationBuilder.RenameColumn(
                name: "HRBstatus",
                table: "DailyMovementReport",
                newName: "HRMstatus");

            migrationBuilder.RenameColumn(
                name: "HRBstatus",
                table: "Campaign",
                newName: "HRMstatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70ae41d4-d8b5-4569-9a03-7a29c6c06bf6", "AQAAAAEAACcQAAAAEPdSWrnVKZXx31E94x0Nrd/MO1iqfZTr372GL+iWwdXNXK4lYbkesnqGiLrom37SNw==", "73731232-1c1d-4a17-97a0-ecc355a57021" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HRMstatus",
                table: "SalesMovement",
                newName: "HRBstatus");

            migrationBuilder.RenameColumn(
                name: "HRMstatus",
                table: "DailyMovementReport",
                newName: "HRBstatus");

            migrationBuilder.RenameColumn(
                name: "HRMstatus",
                table: "Campaign",
                newName: "HRBstatus");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8fb97fac-8781-40ca-9061-f215d69245fb", "AQAAAAEAACcQAAAAEMwIeX7PFj3uL/0b6qKagkbwIw7DicD5tuU5rfPgB7ThC+Udgi9ctJt5ul65tH5QKw==", "a6707bb3-c199-47be-875e-30ad8de84765" });
        }
    }
}
