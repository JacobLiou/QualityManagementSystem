using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10067 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "CurrentAssignment",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "待办人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "当前指派给");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "issue",
                type: "longtext",
                nullable: true,
                comment: "问题序号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "issue");

            migrationBuilder.AlterColumn<long>(
                name: "CurrentAssignment",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "当前指派给",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "待办人");
        }
    }
}
