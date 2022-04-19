using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v103 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_projects");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ssu_issues",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "问题简述",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "问题简述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "Status",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                comment: "问题状态",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "状态");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ssu_issues",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "问题描述",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true,
                oldComment: "问题描述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ssu_issues",
                type: "int",
                nullable: false,
                comment: "问题编号",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "关闭日期");

            migrationBuilder.AddColumn<long>(
                name: "Consequence",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "问题性质");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "提出日期");

            migrationBuilder.AddColumn<long>(
                name: "CreatorId",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "提出人");

            migrationBuilder.AddColumn<DateTime>(
                name: "DisconverTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "发现日期");

            migrationBuilder.AddColumn<long>(
                name: "Discover",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "发现人");

            migrationBuilder.AddColumn<DateTime>(
                name: "DispatchTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "分发日期");

            migrationBuilder.AddColumn<long>(
                name: "Dispatcher",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "分发人");

            migrationBuilder.AddColumn<long>(
                name: "Executor",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "解决人");

            migrationBuilder.AddColumn<DateTime>(
                name: "ForecastSolveTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "预计完成日期");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ssu_issues",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "软删除");

            migrationBuilder.AddColumn<long>(
                name: "IssueClassification",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "问题分类");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "产品编号");

            migrationBuilder.AddColumn<long>(
                name: "ProjectId",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "项目编号");

            migrationBuilder.AddColumn<DateTime>(
                name: "SolveTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "解决日期");

            migrationBuilder.AddColumn<long>(
                name: "Source",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "问题来源");

            migrationBuilder.AddColumn<long>(
                name: "Stage",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "问题模块");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidateTime",
                table: "ssu_issues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "验证日期");

            migrationBuilder.AddColumn<long>(
                name: "Verifier",
                table: "ssu_issues",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "验证人");

            migrationBuilder.AddColumn<string>(
                name: "VerifierPlace",
                table: "ssu_issues",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "验证地点")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Consequence",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "CreateTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "DisconverTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Discover",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "DispatchTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Dispatcher",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Executor",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "ForecastSolveTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "IssueClassification",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "SolveTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "ValidateTime",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "Verifier",
                table: "ssu_issues");

            migrationBuilder.DropColumn(
                name: "VerifierPlace",
                table: "ssu_issues");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "ssu_issues",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "问题简述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "问题简述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ssu_issues",
                type: "int",
                nullable: false,
                comment: "状态",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "问题状态");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ssu_issues",
                type: "longtext",
                nullable: true,
                comment: "问题描述",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "问题描述")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ssu_issues",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "问题编号")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "ssu_projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "问题描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "状态"),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "问题简述")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_projects", x => x.Id);
                },
                comment: "问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
