﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10066 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CloseReason",
                table: "issue_detail",
                type: "longtext",
                nullable: true,
                comment: "关闭原因")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseReason",
                table: "issue_detail");
        }
    }
}