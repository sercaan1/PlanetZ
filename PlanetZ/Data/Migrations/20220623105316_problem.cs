using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class problem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Companies",
                newName: "CompanyLogoPath");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyLogoPath",
                table: "Companies",
                newName: "Logo");
        }
    }
}
