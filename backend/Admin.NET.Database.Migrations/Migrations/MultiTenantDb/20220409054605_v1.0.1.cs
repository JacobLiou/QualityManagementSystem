using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Admin.NET.Database.Migrations.Migrations.MultiTenantDb
{
    public partial class v101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "问题简述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true, comment: "问题描述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "状态")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issues", x => x.Id);
                },
                comment: "问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_tenant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    Name = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "公司名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdminName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "管理员名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Host = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "主机")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "电子邮箱")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "电话")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Connection = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "数据库连接")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Schema = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "架构")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Remark = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "创建时间"),
                    UpdatedTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true, comment: "更新时间"),
                    CreatedUserId = table.Column<long>(type: "bigint", nullable: true, comment: "创建者Id"),
                    CreatedUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "创建者名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdatedUserId = table.Column<long>(type: "bigint", nullable: true, comment: "修改者Id"),
                    UpdatedUserName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "修改者名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除标记")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_tenant", x => x.Id);
                },
                comment: "租户表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "sys_tenant",
                columns: new[] { "Id", "AdminName", "Connection", "CreatedTime", "CreatedUserId", "CreatedUserName", "Email", "Host", "IsDeleted", "Name", "Phone", "Remark", "Schema", "UpdatedTime", "UpdatedUserId", "UpdatedUserName" },
                values: new object[] { 142307070918780L, "SuperAdmin", "", new DateTimeOffset(new DateTime(2021, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), null, null, "515096995@163.com", "", false, "平台开发", "18020030720", null, null, null, null, null });

            migrationBuilder.InsertData(
                table: "sys_tenant",
                columns: new[] { "Id", "AdminName", "Connection", "CreatedTime", "CreatedUserId", "CreatedUserName", "Email", "Host", "IsDeleted", "Name", "Phone", "Remark", "Schema", "UpdatedTime", "UpdatedUserId", "UpdatedUserName" },
                values: new object[] { 142307070918781L, "Admin", "", new DateTimeOffset(new DateTime(2021, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 8, 0, 0, 0)), null, null, "zuohuaijun@163.com", "", false, "公司1租户", "18020030720", null, null, null, null, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_issues");

            migrationBuilder.DropTable(
                name: "sys_tenant");
        }
    }
}
