using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlanetZ.Data.Migrations
{
    public partial class nullableCardId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_CreditCards_CreditCardId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CreditCardId",
                table: "Wallets");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "Wallets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_CreditCards_CreditCardId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_CreditCardId",
                table: "Wallets");

            migrationBuilder.AlterColumn<int>(
                name: "CreditCardId",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
