using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class advanceDeneme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvanceCurrencyD",
                table: "Advances");

            migrationBuilder.DropColumn(
                name: "AdvanceTypeD",
                table: "Advances");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdvanceCurrencyD",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdvanceTypeD",
                table: "Advances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
