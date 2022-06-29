using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V10072 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "ssu_product");

            migrationBuilder.AlterColumn<long>(
                name: "DirectorId",
                table: "ssu_project",
                type: "bigint",
                nullable: true,
                comment: "负责人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "负责人");

            migrationBuilder.AlterColumn<long>(
                name: "DirectorId",
                table: "ssu_product",
                type: "bigint",
                nullable: true,
                comment: "负责人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "负责人");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "DirectorId",
                table: "ssu_project",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "负责人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "负责人");

            migrationBuilder.AlterColumn<long>(
                name: "DirectorId",
                table: "ssu_product",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "负责人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "负责人");

            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "产品型号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
