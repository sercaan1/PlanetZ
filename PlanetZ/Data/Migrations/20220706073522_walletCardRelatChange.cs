using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class walletCardRelatChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_CreditCards_CreditCardId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CreditCardId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "CreditCardId",
                table: "Wallets");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CreditCards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCards_WalletId",
                table: "CreditCards",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCards_Wallets_WalletId",
                table: "CreditCards",
                column: "WalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCards_Wallets_WalletId",
                table: "CreditCards");

            migrationBuilder.DropIndex(
                name: "IX_CreditCards_WalletId",
                table: "CreditCards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CreditCards");

            migrationBuilder.AddColumn<int>(
                name: "CreditCardId",
                table: "Wallets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CreditCardId",
                table: "Wallets",
                column: "CreditCardId",
                unique: true,
                filter: "[CreditCardId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_CreditCards_CreditCardId",
                table: "Wallets",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id");
        }
    }
}
