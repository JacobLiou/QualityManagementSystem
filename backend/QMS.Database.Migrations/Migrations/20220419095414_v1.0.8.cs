using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v108 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ssu_issue_extend_attribute",
                keyColumn: "ValueType",
                keyValue: null,
                column: "ValueType",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "ssu_issue_extend_attribute",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "字段值类型",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldNullable: true,
                oldComment: "字段值类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_issue_extend_attribute",
                keyColumn: "AttributeText",
                keyValue: null,
                column: "AttributeText",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AttributeText",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "字段中文名",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "字段中文名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_issue_extend_attribute",
                keyColumn: "AttibuteName",
                keyValue: null,
                column: "AttibuteName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AttibuteName",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "字段名",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "字段名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_issue",
                keyColumn: "Title",
                keyValue: null,
                column: "Title",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ssu_issue",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "问题简述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "问题简述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValueType",
                table: "ssu_issue_extend_attribute",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true,
                comment: "字段值类型",
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30,
                oldComment: "字段值类型")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AttributeText",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "字段中文名",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "字段中文名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AttibuteName",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "字段名",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "字段名")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ssu_issue",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "问题简述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "问题简述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
