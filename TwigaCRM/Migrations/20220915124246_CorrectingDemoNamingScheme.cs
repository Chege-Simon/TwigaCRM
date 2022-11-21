using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwigaCRM.Migrations
{
    public partial class CorrectingDemoNamingScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoDetail_CropAndAnimal_CropAndAnimalId",
                table: "DemoDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoDetail_PestAndDisease_PestAndDiseaseId",
                table: "DemoDetail");

            migrationBuilder.DropColumn(
                name: "CropOrAnimalId",
                table: "DemoDetail");

            migrationBuilder.DropColumn(
                name: "TargetPestOrDiseaseId",
                table: "DemoDetail");

            migrationBuilder.AlterColumn<int>(
                name: "PestAndDiseaseId",
                table: "DemoDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CropAndAnimalId",
                table: "DemoDetail",
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
                values: new object[] { "b5fe4512-5ae4-4694-a063-117138c191a5", "AQAAAAEAACcQAAAAEE1DMf/91fLCS1PkeW1dVu4OLYG97Xtklb/8AcQnbofcSi0o4kanN3rRmB0dTglcPg==", "56f8b32e-10b9-443a-bf0c-8825608af1f4" });

            migrationBuilder.AddForeignKey(
                name: "FK_DemoDetail_CropAndAnimal_CropAndAnimalId",
                table: "DemoDetail",
                column: "CropAndAnimalId",
                principalTable: "CropAndAnimal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DemoDetail_PestAndDisease_PestAndDiseaseId",
                table: "DemoDetail",
                column: "PestAndDiseaseId",
                principalTable: "PestAndDisease",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DemoDetail_CropAndAnimal_CropAndAnimalId",
                table: "DemoDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_DemoDetail_PestAndDisease_PestAndDiseaseId",
                table: "DemoDetail");

            migrationBuilder.AlterColumn<int>(
                name: "PestAndDiseaseId",
                table: "DemoDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CropAndAnimalId",
                table: "DemoDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CropOrAnimalId",
                table: "DemoDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetPestOrDiseaseId",
                table: "DemoDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b4280b6a-0613-4cbd-a9e6-f1701e926e73",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dcae36b1-1ba8-4d5e-b06e-ff3c8d12eb5d", "AQAAAAEAACcQAAAAELnOvg20cCIqqK+B82SaS3iNAZv3FfpcYHRB9xeRPyfup5WFQRVePyctupN1eRrmTA==", "c42ad473-c04f-4cb4-bf8c-a688f676532e" });

            migrationBuilder.AddForeignKey(
                name: "FK_DemoDetail_CropAndAnimal_CropAndAnimalId",
                table: "DemoDetail",
                column: "CropAndAnimalId",
                principalTable: "CropAndAnimal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DemoDetail_PestAndDisease_PestAndDiseaseId",
                table: "DemoDetail",
                column: "PestAndDiseaseId",
                principalTable: "PestAndDisease",
                principalColumn: "Id");
        }
    }
}
