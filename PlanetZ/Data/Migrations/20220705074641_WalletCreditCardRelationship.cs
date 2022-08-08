using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class WalletCreditCardRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditCardWallet");

            migrationBuilder.AddColumn<int>(
                name: "CreditCardId",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                table: "CreditCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CreditCardId",
                table: "Wallets",
                column: "CreditCardId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_CreditCards_CreditCardId",
                table: "Wallets",
                column: "CreditCardId",
                principalTable: "CreditCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "CreditCards");

            migrationBuilder.CreateTable(
                name: "CreditCardWallet",
                columns: table => new
                {
                    CreditCardsId = table.Column<int>(type: "int", nullable: false),
                    WalletsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardWallet", x => new { x.CreditCardsId, x.WalletsId });
                    table.ForeignKey(
                        name: "FK_CreditCardWallet_CreditCards_CreditCardsId",
                        column: x => x.CreditCardsId,
                        principalTable: "CreditCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditCardWallet_Wallets_WalletsId",
                        column: x => x.WalletsId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardWallet_WalletsId",
                table: "CreditCardWallet",
                column: "WalletsId");
        }
    }
}
