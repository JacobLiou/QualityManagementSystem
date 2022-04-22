using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1012 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_detail_ssu_issue_Id",
                table: "ssu_issue_detail");

            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_extend_attribute_~",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_extend_attribute_value_ssu_issue_IssueId",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.DropIndex(
                name: "IX_ssu_issue_extend_attribute_value_IssueId",
                table: "ssu_issue_extend_attribute_value");

            migrationBuilder.RenameColumn(
                name: "DisconverTime",
                table: "ssu_issue",
                newName: "DiscoverTime");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_detail",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "HangupReason",
                table: "ssu_issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "挂起情况")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HangupReason",
                table: "ssu_issue_detail");

            migrationBuilder.RenameColumn(
                name: "DiscoverTime",
                table: "ssu_issue",
                newName: "DisconverTime");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "ssu_issue_detail",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_ssu_issue_extend_attribute_value_IssueId",
                table: "ssu_issue_extend_attribute_value",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ssu_issue_detail_ssu_issue_Id",
                table: "ssu_issue_detail",
                column: "Id",
                principalTable: "ssu_issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
