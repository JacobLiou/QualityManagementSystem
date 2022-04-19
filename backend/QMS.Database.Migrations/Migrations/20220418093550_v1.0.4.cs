using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class v104 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ssu_product_users",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false, comment: "产品编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "参与人员")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_product_users", x => x.ProductId);
                },
                comment: "产品人员关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "产品编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductLine = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品线")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "所属项目")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "状态")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false, comment: "分类"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_products", x => x.Id);
                },
                comment: "产品表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_project_users",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false, comment: "项目编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "参与人员")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_project_users", x => x.ProjectId);
                },
                comment: "项目人员关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_projects",
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
                    table.PrimaryKey("PK_ssu_projects", x => x.Id);
                },
                comment: "项目表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_product_users");

            migrationBuilder.DropTable(
                name: "ssu_products");

            migrationBuilder.DropTable(
                name: "ssu_project_users");

            migrationBuilder.DropTable(
                name: "ssu_projects");
        }
    }
}
