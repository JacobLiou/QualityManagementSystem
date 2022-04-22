using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v109 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "ssu_issue_detail",
                comment: "详细问题记录",
                oldComment: "问题记录")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_issue_extend_attribute_value",
                keyColumn: "AttibuteName",
                keyValue: null,
                column: "AttibuteName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AttibuteName",
                table: "ssu_issue_extend_attribute_value",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "字段值",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "字段值")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ssu_issue_detail_ssu_issue_Id",
                table: "ssu_issue_detail",
                column: "Id",
                principalTable: "ssu_issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ssu_issue_detail_ssu_issue_Id",
                table: "ssu_issue_detail");

            migrationBuilder.AlterTable(
                name: "ssu_issue_detail",
                comment: "问题记录",
                oldComment: "详细问题记录")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "AttibuteName",
                table: "ssu_issue_extend_attribute_value",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "字段值",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "字段值")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
        }
    }
}
