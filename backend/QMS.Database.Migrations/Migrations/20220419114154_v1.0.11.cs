using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class v1011 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ssu_project",
                keyColumn: "ProjectName",
                keyValue: null,
                column: "ProjectName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "ssu_project",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "项目名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "项目名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_product",
                keyColumn: "ProductType",
                keyValue: null,
                column: "ProductType",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "产品型号",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "产品型号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_product",
                keyColumn: "ProductName",
                keyValue: null,
                column: "ProductName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ssu_product",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "产品名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_product",
                keyColumn: "ProductLine",
                keyValue: null,
                column: "ProductLine",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ProductLine",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "产品线",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "产品线")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "ssu_group",
                keyColumn: "GroupName",
                keyValue: null,
                column: "GroupName",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ssu_group",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                comment: "人员组名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "人员组名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProjectName",
                table: "ssu_project",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "项目名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "项目名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "产品型号",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "产品型号")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "ssu_product",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "产品名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "产品名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ProductLine",
                table: "ssu_product",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                comment: "产品线",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "产品线")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "GroupName",
                table: "ssu_group",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                comment: "人员组名称",
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldComment: "人员组名称")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
