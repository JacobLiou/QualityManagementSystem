using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V1020 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_operation",
                type: "bigint",
                nullable: false,
                comment: "问题操作记录编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题操作记录编号")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
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
                oldComment: "问题操作记录编号")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
