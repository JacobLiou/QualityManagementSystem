using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class V10044 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_version",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    Version = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "版本号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "创建时间"),
                    UpdatedTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "更新时间"),
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
                    table.PrimaryKey("PK_sys_version", x => x.Id);
                },
                comment: "版本表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_version");
        }
    }
}
