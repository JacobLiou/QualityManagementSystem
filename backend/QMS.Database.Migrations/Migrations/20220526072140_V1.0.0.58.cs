using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10058 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_issue_ext_attr_val_Id_IssueNum",
                table: "issue_ext_attr_val",
                columns: new string[] { "Id" , "IssueNum" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_issue_ext_attr_val_Id_IssueNum",
                table: "issue_ext_attr_val");
        }
    }
}
