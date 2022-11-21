using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingCompetetionIntelligence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompetitionActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewProduct = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceChanges = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeInDeployment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Launches = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingFarmers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainingStockists = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaPresence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockOuts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RebatsAndSchemes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MajorPurchaseFromCompetition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompetitionActivity_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7f84f54e-ef20-48aa-9ad4-dc26e80c268f", "AQAAAAEAACcQAAAAECMLNBTJrgBdrpiefnmciOz7OfwNo3Ioima9L82u+0O5dF21NqkKvJJW0V7Ne1zsXw==", "b56cd9ce-cccc-4b5c-ac80-e3c4aeb053ad" });

            migrationBuilder.CreateIndex(
                name: "IX_CompetitionActivity_AppUserId",
                table: "CompetitionActivity",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompetitionActivity");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "70ae41d4-d8b5-4569-9a03-7a29c6c06bf6", "AQAAAAEAACcQAAAAEPdSWrnVKZXx31E94x0Nrd/MO1iqfZTr372GL+iWwdXNXK4lYbkesnqGiLrom37SNw==", "73731232-1c1d-4a17-97a0-ecc355a57021" });
        }
    }
}
