﻿using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class v1017 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IssueId",
                table: "ssu_issue_extend_attribute_value",
                newName: "IssueNum");

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

            migrationBuilder.RenameColumn(
                name: "IssueNum",
                table: "ssu_issue_extend_attribute_value",
                newName: "IssueId");

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