using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V1019 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "ssu_project");

            migrationBuilder.AlterColumn<string>(
                name: "NetType",
                table: "sys_code_gen_config",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: ".NET数据类型",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: ".NET数据类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NetType",
                table: "sys_code_gen_config",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: ".NET数据类型",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: ".NET数据类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "ssu_project",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                comment: "项目编号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
