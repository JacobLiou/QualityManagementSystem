using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V10071 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassificationId",
                table: "ssu_product",
                type: "int",
                nullable: true,
                comment: "产品分类",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "产品分类");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClassificationId",
                table: "ssu_product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "产品分类",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "产品分类");
        }
    }
}
