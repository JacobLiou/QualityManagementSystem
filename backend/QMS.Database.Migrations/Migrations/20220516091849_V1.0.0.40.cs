using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10040 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CC",
                table: "issue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CC",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "被抄送人");
        }
    }
}
