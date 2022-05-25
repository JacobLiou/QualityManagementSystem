using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V10055 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductLine",
                table: "ssu_product",
                type: "int",
                maxLength: 100,
                nullable: false,
                comment: "产品线",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "产品线")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductLine",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "产品线",
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100,
                oldComment: "产品线")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
