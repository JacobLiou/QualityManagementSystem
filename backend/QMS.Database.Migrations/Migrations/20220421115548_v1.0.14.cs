using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1014 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeText",
                table: "ssu_issue_extend_attribute");

            migrationBuilder.AddColumn<string>(
                name: "AttributeCode",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "字段代码")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "CC",
                table: "ssu_issue",
                type: "bigint",
                nullable: true,
                comment: "被抄送人");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttributeCode",
                table: "ssu_issue_extend_attribute");

            migrationBuilder.DropColumn(
                name: "CC",
                table: "ssu_issue");

            migrationBuilder.AddColumn<string>(
                name: "AttributeText",
                table: "ssu_issue_extend_attribute",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                comment: "字段中文名")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
