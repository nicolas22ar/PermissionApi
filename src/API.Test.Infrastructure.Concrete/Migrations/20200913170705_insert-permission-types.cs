using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Test.Infrastructure.Concrete.Migrations
{
    public partial class insertpermissiontypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "PermissionType", schema: "dbo", columns: new[] { "Description" }, values: new object[] { "Sickness" });
            migrationBuilder.InsertData(table: "PermissionType", schema: "dbo", columns: new[] { "Description" }, values: new object[] { "Personal Time Off" });
            migrationBuilder.InsertData(table: "PermissionType", schema: "dbo", columns: new[] { "Description" }, values: new object[] { "Vacations" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE Table dbo.PermissionType");
        }
    }
}