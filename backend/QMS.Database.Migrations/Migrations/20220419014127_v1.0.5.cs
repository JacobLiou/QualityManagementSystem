using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class v105 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sys_test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "项目编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "项目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_test", x => x.Id);
                },
                comment: "项目表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "sys_test");
        }
    }
}
