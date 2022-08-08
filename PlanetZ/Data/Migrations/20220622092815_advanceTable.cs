using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class advanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advance_AspNetUsers_EmployeeId",
                table: "Advance");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advance",
                table: "Advance");

            migrationBuilder.RenameTable(
                name: "Advance",
                newName: "Advances");

            migrationBuilder.RenameIndex(
                name: "IX_Advance_EmployeeId",
                table: "Advances",
                newName: "IX_Advances_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advances",
                table: "Advances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advances_AspNetUsers_EmployeeId",
                table: "Advances",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advances_AspNetUsers_EmployeeId",
                table: "Advances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Advances",
                table: "Advances");

            migrationBuilder.RenameTable(
                name: "Advances",
                newName: "Advance");

            migrationBuilder.RenameIndex(
                name: "IX_Advances_EmployeeId",
                table: "Advance",
                newName: "IX_Advance_EmployeeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Advance",
                table: "Advance",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Advance_AspNetUsers_EmployeeId",
                table: "Advance",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
