using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10053 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "issue",
                type: "int",
                nullable: true,
                comment: "问题来源",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "问题来源");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Source",
                table: "issue",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "问题来源",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "问题来源");
        }
    }
}
