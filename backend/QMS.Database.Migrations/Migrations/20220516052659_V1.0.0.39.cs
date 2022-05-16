using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10039 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SolveVersion",
                table: "issue_detail",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "解决版本",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "解决版本")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "验证情况",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "验证情况")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HangupReason",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "挂起情况",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "挂起情况")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ExtendAttribute",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "扩展属性",
                oldClrType: typeof(string),
                oldType: "varchar(1500)",
                oldMaxLength: 1500,
                oldNullable: true,
                oldComment: "扩展属性")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "问题详情",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "问题详情")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "备注")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "附件信息",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "附件信息")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CCs",
                table: "issue",
                type: "longtext",
                nullable: true,
                comment: "被抄送人")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCs",
                table: "issue");

            migrationBuilder.AlterColumn<string>(
                name: "SolveVersion",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "解决版本",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "解决版本")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Result",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "验证情况",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "验证情况")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "HangupReason",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "挂起情况",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "挂起情况")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ExtendAttribute",
                table: "issue_detail",
                type: "varchar(1500)",
                maxLength: 1500,
                nullable: true,
                comment: "扩展属性",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "扩展属性")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "问题详情",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "问题详情")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "issue_detail",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "备注",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "备注")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Attachments",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "附件信息",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "附件信息")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
