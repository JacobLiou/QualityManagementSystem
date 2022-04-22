using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1010 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IssueId",
                table: "ssu_issue_extend_attribute_value",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "模块编号");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_extend_attribute_value",
                type: "bigint",
                nullable: false,
                comment: "字段编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "字段编号")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_ssu_issue_extend_attribute_value_IssueId",
                table: "ssu_issue_extend_attribute_value",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_extend_attribute_~",
                table: "ssu_issue_extend_attribute_value",
                column: "Id",
                principalTable: "ssu_issue_extend_attribute",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_IssueId",
                table: "ssu_issue_extend_attribute_value",
                column: "IssueId",
                principalTable: "ssu_issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_extend_attribute_~",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_IssueId",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_ssu_issue_extend_attribute_value_IssueId",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.AlterColumn<long>(
                name: "IssueId",
                table: "ssu_issue_extend_attribute_value",
                type: "bigint",
                nullable: false,
                comment: "模块编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_extend_attribute_value",
                type: "bigint",
                nullable: false,
                comment: "字段编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "字段编号")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }
    }
}
