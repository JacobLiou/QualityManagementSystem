using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10043 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrentAssignment",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "当前指派给");

            migrationBuilder.AddColumn<int>(
                name: "ValidationStatus",
                table: "issue",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "回归验证状态");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentAssignment",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "ValidationStatus",
                table: "issue");
        }
    }
}
