using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v10062 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "issue",
                type: "tinyint(1)",
                nullable: false,
                comment: "软删除标记",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "软删除");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "issue",
                type: "bigint",
                nullable: false,
                comment: "Id主键",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题编号");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "issue",
                type: "datetime(6)",
                nullable: true,
                comment: "创建时间");

            migrationBuilder.AddColumn<long>(
                name: "CreatedUserId",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "创建者Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "issue",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "创建者名称")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedTime",
                table: "issue",
                type: "datetime(6)",
                nullable: true,
                comment: "更新时间");

            migrationBuilder.AddColumn<long>(
                name: "UpdatedUserId",
                table: "issue",
                type: "bigint",
                nullable: true,
                comment: "修改者Id");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUserName",
                table: "issue",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "修改者名称")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "issue");

            migrationBuilder.DropColumn(
                name: "UpdatedUserName",
                table: "issue");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "issue",
                type: "tinyint(1)",
                nullable: false,
                comment: "软删除",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "软删除标记");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "issue",
                type: "bigint",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "Id主键");
        }
    }
}
