using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    public partial class V10038 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Columns",
                table: "issue_column_display",
                type: "varchar(600)",
                maxLength: 600,
                nullable: true,
                comment: "列名集合",
                oldClrType: typeof(string),
                oldType: "varchar(300)",
                oldMaxLength: 300,
                oldNullable: true,
                oldComment: "列名集合")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Columns",
                table: "issue_column_display",
                type: "varchar(300)",
                maxLength: 300,
                nullable: true,
                comment: "列名集合",
                oldClrType: typeof(string),
                oldType: "varchar(600)",
                oldMaxLength: 600,
                oldNullable: true,
                oldComment: "列名集合")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
