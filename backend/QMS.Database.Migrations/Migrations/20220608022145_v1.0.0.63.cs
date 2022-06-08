using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v10063 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedTime",
                table: "issue_ext_attr",
                type: "datetime(6)",
                nullable: true,
                comment: "创建时间");

            migrationBuilder.AddColumn<long>(
                name: "CreatedUserId",
                table: "issue_ext_attr",
                type: "bigint",
                nullable: true,
                comment: "创建者Id");

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserName",
                table: "issue_ext_attr",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "创建者名称")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "issue_ext_attr",
                type: "bigint",
                nullable: true,
                comment: "租户id");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedTime",
                table: "issue_ext_attr",
                type: "datetime(6)",
                nullable: true,
                comment: "更新时间");

            migrationBuilder.AddColumn<long>(
                name: "UpdatedUserId",
                table: "issue_ext_attr",
                type: "bigint",
                nullable: true,
                comment: "修改者Id");

            migrationBuilder.AddColumn<string>(
                name: "UpdatedUserName",
                table: "issue_ext_attr",
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
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "CreatedUserName",
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "UpdatedTime",
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "issue_ext_attr");

            migrationBuilder.DropColumn(
                name: "UpdatedUserName",
                table: "issue_ext_attr");
        }
    }
}
