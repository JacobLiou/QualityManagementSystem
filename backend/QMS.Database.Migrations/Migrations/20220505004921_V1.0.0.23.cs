using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_operation",
                type: "bigint",
                nullable: false,
                comment: "Id主键",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题操作记录编号");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_operation",
                type: "bigint",
                nullable: false,
                comment: "问题操作记录编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Id主键");
        }
    }
}
