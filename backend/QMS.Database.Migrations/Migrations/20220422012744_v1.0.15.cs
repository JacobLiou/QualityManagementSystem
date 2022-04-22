using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1015 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_operation_ssu_issue_IssueId",
                table: "ssu_issue_operation");

            migrationBuilder.DropIndex(
                name: "IX_ssu_issue_operation_IssueId",
                table: "ssu_issue_operation");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ssu_issue_operation_IssueId",
                table: "ssu_issue_operation",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ssu_issue_operation_ssu_issue_IssueId",
                table: "ssu_issue_operation",
                column: "IssueId",
                principalTable: "ssu_issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
