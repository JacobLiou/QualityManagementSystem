using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V10069 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductLine",
                table: "ssu_product");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ssu_product");

            migrationBuilder.AddColumn<string>(
                name: "CloseReason",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "关闭原因")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "CurrentAssignment",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "待办人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "当前指派给");

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "issue",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "问题序号")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseReason",
                table: "issue_detail");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "issue");

            migrationBuilder.AddColumn<int>(
                name: "ProductLine",
                table: "ssu_product",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0,
                comment: "产品线");

            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "ssu_product",
                type: "bigint",
                maxLength: 100,
                nullable: false,
                defaultValue: 0L,
                comment: "所属项目");

            migrationBuilder.AlterColumn<long>(
                name: "CurrentAssignment",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "当前指派给",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "待办人");
        }
    }
}
