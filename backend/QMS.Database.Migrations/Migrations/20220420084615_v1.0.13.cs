using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1013 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttibuteName",
                table: "ssu_issue_extend_attribute_value",
                newName: "AttibuteValue");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "ssu_issue_detail",
                type: "int",
                nullable: true,
                comment: "验证数量",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "验证数量");

            migrationBuilder.AlterColumn<long>(
                name: "Verifier",
                table: "ssu_issue",
                type: "bigint",
                nullable: true,
                comment: "验证人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "验证人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidateTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "验证日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "验证日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SolveTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "解决日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "解决日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ForecastSolveTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "预计完成日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "预计完成日期");

            migrationBuilder.AlterColumn<long>(
                name: "Executor",
                table: "ssu_issue",
                type: "bigint",
                nullable: true,
                comment: "解决人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "解决人");

            migrationBuilder.AlterColumn<long>(
                name: "Dispatcher",
                table: "ssu_issue",
                type: "bigint",
                nullable: true,
                comment: "分发人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "分发人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DispatchTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "分发日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "分发日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscoverTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "发现日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "发现日期");

            migrationBuilder.AlterColumn<long>(
                name: "Discover",
                table: "ssu_issue",
                type: "bigint",
                nullable: true,
                comment: "发现人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldComment: "发现人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CloseTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: true,
                comment: "关闭日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldComment: "关闭日期");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttibuteValue",
                table: "ssu_issue_extend_attribute_value",
                newName: "AttibuteName");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "ssu_issue_detail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "验证数量",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "验证数量");

            migrationBuilder.AlterColumn<long>(
                name: "Verifier",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "验证人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "验证人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidateTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "验证日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "验证日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SolveTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "解决日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "解决日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ForecastSolveTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "预计完成日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "预计完成日期");

            migrationBuilder.AlterColumn<long>(
                name: "Executor",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "解决人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "解决人");

            migrationBuilder.AlterColumn<long>(
                name: "Dispatcher",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "分发人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "分发人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DispatchTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "分发日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "分发日期");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DiscoverTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "发现日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "发现日期");

            migrationBuilder.AlterColumn<long>(
                name: "Discover",
                table: "ssu_issue",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "发现人",
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true,
                oldComment: "发现人");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CloseTime",
                table: "ssu_issue",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                comment: "关闭日期",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldComment: "关闭日期");
        }
    }
}
