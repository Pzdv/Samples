using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hwEF.Migrations
{
    public partial class RenameRoleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Role", "dbo", "Roles", "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable("Roles", "dbo", "Role", "dbo");
        }
    }
}
