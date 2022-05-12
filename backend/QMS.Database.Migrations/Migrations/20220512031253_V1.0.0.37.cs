using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10037 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_issue_extend_attribute",
                table: "issue_extend_attribute");

            migrationBuilder.RenameTable(
                name: "issue_extend_attribute",
                newName: "issue_ext_attr");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issue_ext_attr",
                table: "issue_ext_attr",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_issue_ext_attr",
                table: "issue_ext_attr");

            migrationBuilder.RenameTable(
                name: "issue_ext_attr",
                newName: "issue_extend_attribute");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issue_extend_attribute",
                table: "issue_extend_attribute",
                column: "Id");
        }
    }
}
