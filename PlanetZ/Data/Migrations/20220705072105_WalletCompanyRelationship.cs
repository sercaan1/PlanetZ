using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class WalletCompanyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_EmployeeId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_EmployeeId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CompanyId",
                table: "Wallets",
                column: "CompanyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Companies_CompanyId",
                table: "Wallets",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Companies_CompanyId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CompanyId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Wallets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_EmployeeId",
                table: "Wallets",
                column: "EmployeeId",
                unique: true,
                filter: "[EmployeeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_EmployeeId",
                table: "Wallets",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
