using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class addingPestandDiseases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "ProductCropAndAnimal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "ProductCropAndAnimal",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "CropAndAnimalPestAndDisease",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateAt",
                table: "CropAndAnimalPestAndDisease",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f25ac3e2-8ad1-4cc9-afc5-be121ee352ce", "AQAAAAEAACcQAAAAEEJhBKQ8dg0LaSyfPPhW1/0pLMSE1DNz2cM6adYM0/I1prgVTrgD2Ih2GbpTdIp5rA==", "599e7489-da7e-48ef-bdd9-2c9c258019da" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "ProductCropAndAnimal");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "ProductCropAndAnimal");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "CropAndAnimalPestAndDisease");

            migrationBuilder.DropColumn(
                name: "UpdateAt",
                table: "CropAndAnimalPestAndDisease");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7a88c97a-d4c3-421d-9bff-5b47d4f77394", "AQAAAAEAACcQAAAAEIbx/rjez99CLYYJIblvhC2ZYhAx4WU6TOLHoUsJHkyoTyqvrpcblYbgr+0QSs+Fqg==", "239c4c83-5630-4e66-aec1-2072108378ea" });
        }
    }
}
