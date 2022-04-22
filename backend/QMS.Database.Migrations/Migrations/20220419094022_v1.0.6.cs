using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class v106 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_product_users");

            migrationBuilder.DropTable(
                name: "ssu_products");

            migrationBuilder.DropTable(
                name: "ssu_project_users");

            migrationBuilder.DropTable(
                name: "ssu_projects");

            migrationBuilder.DropTable(
                name: "sys_test");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentType",
                table: "sys_file",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "附件来源");

            migrationBuilder.AddColumn<long>(
                name: "IssueId",
                table: "sys_file",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                comment: "问题编号");

            migrationBuilder.CreateTable(
                name: "ssu_group",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    GroupName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "人员组名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
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
                    table.PrimaryKey("PK_ssu_group", x => x.Id);
                },
                comment: "人员组表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    ProductName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品型号")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductLine = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品线")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<long>(type: "bigint", maxLength: 100, nullable: false, comment: "所属项目"),
                    Status = table.Column<int>(type: "int", maxLength: 100, nullable: false, comment: "状态"),
                    ClassificationId = table.Column<int>(type: "int", nullable: false, comment: "产品分类"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
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
                    table.PrimaryKey("PK_ssu_product", x => x.Id);
                },
                comment: "产品表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    ProjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "项目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
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
                    table.PrimaryKey("PK_ssu_project", x => x.Id);
                },
                comment: "项目表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_group_user",
                columns: table => new
                {
                    GroupId = table.Column<long>(type: "bigint", nullable: false, comment: "人员组编号"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "成员编号")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_group_user", x => new { x.GroupId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_ssu_group_user_ssu_group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ssu_group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ssu_group_user_sys_emp_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "sys_emp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "人员组成员关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_product_user",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false, comment: "产品编号"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "参与人员")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_product_user", x => new { x.ProductId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_ssu_product_user_ssu_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ssu_product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ssu_product_user_sys_emp_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "sys_emp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "产品人员关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_project_product",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false, comment: "项目编号"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false, comment: "产品编号")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_project_product", x => new { x.ProjectId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ssu_project_product_ssu_product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ssu_product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ssu_project_product_ssu_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ssu_project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "项目产品关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_project_user",
                columns: table => new
                {
                    ProjectId = table.Column<long>(type: "bigint", nullable: false, comment: "项目编号"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false, comment: "参与人员")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_project_user", x => new { x.ProjectId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_ssu_project_user_ssu_project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "ssu_project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ssu_project_user_sys_emp_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "sys_emp",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "项目人员关联表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ssu_group_user_EmployeeId",
                table: "ssu_group_user",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ssu_product_user_EmployeeId",
                table: "ssu_product_user",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ssu_project_product_ProductId",
                table: "ssu_project_product",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ssu_project_user_EmployeeId",
                table: "ssu_project_user",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_group_user");

            migrationBuilder.DropTable(
                name: "ssu_product_user");

            migrationBuilder.DropTable(
                name: "ssu_project_product");

            migrationBuilder.DropTable(
                name: "ssu_project_user");

            migrationBuilder.DropTable(
                name: "ssu_group");

            migrationBuilder.DropTable(
                name: "ssu_product");

            migrationBuilder.DropTable(
                name: "ssu_project");

            migrationBuilder.DropColumn(
                name: "AttachmentType",
                table: "sys_file");

            migrationBuilder.DropColumn(
                name: "IssueId",
                table: "sys_file");

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
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false, comment: "分类"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    ProductLine = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品线")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "产品名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "产品类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "所属项目")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    Status = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "状态")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间")
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
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    ProjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "项目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_projects", x => x.Id);
                },
                comment: "项目表")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "sys_test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "项目编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    DirectorId = table.Column<long>(type: "bigint", nullable: false, comment: "负责人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    ProjectName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "项目名称")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "修改时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sys_test", x => x.Id);
                },
                comment: "项目表")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
