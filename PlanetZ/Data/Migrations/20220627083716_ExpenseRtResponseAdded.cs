using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class ExpenseRtResponseAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturnResponse",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnResponse",
                table: "Expenses");
        }
    }
}
