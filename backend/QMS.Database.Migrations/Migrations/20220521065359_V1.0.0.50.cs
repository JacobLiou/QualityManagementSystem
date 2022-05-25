using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10050 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_issue_ext_attr_val_issue_ext_attr_Id",
                table: "issue_ext_attr_val",
                column: "Id",
                principalTable: "issue_ext_attr",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_issue_ext_attr_val_issue_ext_attr_Id",
                table: "issue_ext_attr_val");
        }
    }
}
