using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10035 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ssu_issue_detail");

            migrationBuilder.DropTable(
                name: "ssu_issue_extend_attribute");

            migrationBuilder.DropTable(
                name: "ssu_issue_extend_attribute_value");

            migrationBuilder.DropTable(
                name: "ssu_issue_operation");

            migrationBuilder.DropTable(
                name: "ssu_issue");

            migrationBuilder.CreateTable(
                name: "issue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "问题简述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false, comment: "项目编号"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false, comment: "产品编号"),
                    Module = table.Column<int>(type: "int", nullable: false, comment: "问题模块"),
                    Consequence = table.Column<int>(type: "int", nullable: false, comment: "问题性质"),
                    IssueClassification = table.Column<int>(type: "int", nullable: false, comment: "问题分类"),
                    Source = table.Column<int>(type: "int", nullable: false, comment: "问题来源"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "问题状态"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "提出人"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "提出日期"),
                    CloseTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "关闭日期"),
                    Discover = table.Column<long>(type: "bigint", nullable: true, comment: "发现人"),
                    DiscoverTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "发现日期"),
                    Dispatcher = table.Column<long>(type: "bigint", nullable: true, comment: "分发人"),
                    DispatchTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "分发日期"),
                    ForecastSolveTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "预计完成日期"),
                    CC = table.Column<long>(type: "bigint", nullable: true, comment: "被抄送人"),
                    Executor = table.Column<long>(type: "bigint", nullable: true, comment: "解决人"),
                    SolveTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "解决日期"),
                    Verifier = table.Column<long>(type: "bigint", nullable: true, comment: "验证人"),
                    VerifierPlace = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "验证地点")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValidateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "验证日期"),
                    HangupId = table.Column<long>(type: "bigint", nullable: true, comment: "挂起人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue", x => x.Id);
                },
                comment: "问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issue_column_display",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false, comment: "用户编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Columns = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "列名集合")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_column_display", x => x.UserId);
                },
                comment: "问题列表显示列明记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issue_ext_attr_val",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "字段编号"),
                    IssueNum = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    AttibuteValue = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "字段值")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_ext_attr_val", x => new { x.Id, x.IssueNum });
                },
                comment: "问题扩展属性值")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issue_extend_attribute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "字段编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Module = table.Column<int>(type: "int", nullable: false, comment: "模块编号"),
                    AttibuteName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "字段名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttributeCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "字段代码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValueType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "字段值类型")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    UpdateId = table.Column<long>(type: "bigint", nullable: false, comment: "更新人"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "提出日期"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序优先级"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_extend_attribute", x => x.Id);
                },
                comment: "问题扩展属性")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issue_operation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    IssueId = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    OperationTypeId = table.Column<int>(type: "int", maxLength: 200, nullable: false, comment: "操作类型"),
                    OperatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "操作人")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: true, comment: "内容")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "时间"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_operation", x => x.Id);
                },
                comment: "问题操作记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "issue_detail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "问题详情")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "原因分析")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Measures = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "解决措施")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Count = table.Column<int>(type: "int", nullable: true, comment: "验证数量"),
                    Batch = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "验证批次")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "验证情况")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SolveVersion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "解决版本")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HangupReason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "挂起情况")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtendAttribute = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true, comment: "扩展属性")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Attachments = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "附件信息")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issue_detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_issue_detail_issue_Id",
                        column: x => x.Id,
                        principalTable: "issue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "详细问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "issue_column_display");

            migrationBuilder.DropTable(
                name: "issue_detail");

            migrationBuilder.DropTable(
                name: "issue_ext_attr_val");

            migrationBuilder.DropTable(
                name: "issue_extend_attribute");

            migrationBuilder.DropTable(
                name: "issue_operation");

            migrationBuilder.DropTable(
                name: "issue");

            migrationBuilder.CreateTable(
                name: "ssu_issue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    CC = table.Column<long>(type: "bigint", nullable: true, comment: "被抄送人"),
                    CloseTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "关闭日期"),
                    Consequence = table.Column<int>(type: "int", nullable: false, comment: "问题性质"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "提出日期"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "提出人"),
                    Discover = table.Column<long>(type: "bigint", nullable: true, comment: "发现人"),
                    DiscoverTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "发现日期"),
                    DispatchTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "分发日期"),
                    Dispatcher = table.Column<long>(type: "bigint", nullable: true, comment: "分发人"),
                    Executor = table.Column<long>(type: "bigint", nullable: true, comment: "解决人"),
                    ForecastSolveTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "预计完成日期"),
                    HangupId = table.Column<long>(type: "bigint", nullable: true, comment: "挂起人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    IssueClassification = table.Column<int>(type: "int", nullable: false, comment: "问题分类"),
                    Module = table.Column<int>(type: "int", nullable: false, comment: "问题模块"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false, comment: "产品编号"),
                    ProjectId = table.Column<long>(type: "bigint", nullable: false, comment: "项目编号"),
                    SolveTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "解决日期"),
                    Source = table.Column<int>(type: "int", nullable: false, comment: "问题来源"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "问题状态"),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "问题简述")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ValidateTime = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "验证日期"),
                    Verifier = table.Column<long>(type: "bigint", nullable: true, comment: "验证人"),
                    VerifierPlace = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true, comment: "验证地点")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issue", x => x.Id);
                },
                comment: "问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_issue_extend_attribute",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "字段编号")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttibuteName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "字段名")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AttributeCode = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "字段代码")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "创建时间"),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false, comment: "创建人"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    Module = table.Column<int>(type: "int", nullable: false, comment: "模块编号"),
                    Sort = table.Column<int>(type: "int", nullable: false, comment: "排序优先级"),
                    UpdateId = table.Column<long>(type: "bigint", nullable: false, comment: "更新人"),
                    UpdateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "提出日期"),
                    ValueType = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, comment: "字段值类型")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issue_extend_attribute", x => x.Id);
                },
                comment: "问题扩展属性")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_issue_extend_attribute_value",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "字段编号"),
                    IssueNum = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    AttibuteValue = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "字段值")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issue_extend_attribute_value", x => new { x.Id, x.IssueNum });
                },
                comment: "问题扩展属性值")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_issue_operation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "Id主键"),
                    Content = table.Column<string>(type: "longtext", nullable: true, comment: "内容")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "软删除"),
                    IssueId = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    OperationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, comment: "时间"),
                    OperationTypeId = table.Column<int>(type: "int", maxLength: 200, nullable: false, comment: "操作类型"),
                    OperatorName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "操作人")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issue_operation", x => x.Id);
                },
                comment: "问题操作记录")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ssu_issue_detail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, comment: "问题编号"),
                    Attachments = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "附件信息")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Batch = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "验证批次")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Comment = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true, comment: "备注")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Count = table.Column<int>(type: "int", nullable: true, comment: "验证数量"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "问题详情")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtendAttribute = table.Column<string>(type: "varchar(1500)", maxLength: 1500, nullable: true, comment: "扩展属性")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HangupReason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "挂起情况")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Measures = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "解决措施")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "原因分析")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Result = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "验证情况")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SolveVersion = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "解决版本")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ssu_issue_detail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ssu_issue_detail_ssu_issue_Id",
                        column: x => x.Id,
                        principalTable: "ssu_issue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "详细问题记录")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
