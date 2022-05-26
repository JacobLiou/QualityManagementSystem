using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10056 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_issue_ext_attr_val",
                table: "issue_ext_attr_val");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "原因分析",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "原因分析")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Measures",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "解决措施",
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true,
                oldComment: "解决措施")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "产品编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "产品编号");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issue_ext_attr_val",
                table: "issue_ext_attr_val",
                columns: new[] { "IssueNum", "Id" });

            //migrationBuilder.CreateIndex(
            //    name: "IX_issue_ext_attr_val_Id",
            //    table: "issue_ext_attr_val",
            //    column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_issue_ext_attr_val_issue_IssueNum",
                table: "issue_ext_attr_val",
                column: "IssueNum",
                principalTable: "issue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_issue_ext_attr_val_issue_IssueNum",
                table: "issue_ext_attr_val");

            migrationBuilder.DropForeignKey(
                name: "FK_issue_ext_attr_val_issue_ext_attr_Id",
                table: "issue_ext_attr_val");

            migrationBuilder.DropPrimaryKey(
                name: "PK_issue_ext_attr_val",
                table: "issue_ext_attr_val");

            //migrationBuilder.DropIndex(
            //    name: "IX_issue_ext_attr_val_Id",
            //    table: "issue_ext_attr_val");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "原因分析",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "原因分析")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Measures",
                table: "issue_detail",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "解决措施",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "解决措施")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "issue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "产品编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "产品编号");

            migrationBuilder.AddPrimaryKey(
                name: "PK_issue_ext_attr_val",
                table: "issue_ext_attr_val",
                columns: new[] { "Id", "IssueNum" });
        }
    }
}
