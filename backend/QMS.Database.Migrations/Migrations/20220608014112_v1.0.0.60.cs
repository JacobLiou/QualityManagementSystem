using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QMS.Database.Migrations.Migrations.DefaultDb
{
    public partial class v10060 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "ssu_project",
                type: "bigint",
                nullable: true,
                comment: "租户id");

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "ssu_product",
                type: "bigint",
                nullable: true,
                comment: "租户id");

            migrationBuilder.AddColumn<long>(
                name: "TenantId",
                table: "ssu_group",
                type: "bigint",
                nullable: true,
                comment: "租户id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ssu_project");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ssu_product");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ssu_group");
        }
    }
}
