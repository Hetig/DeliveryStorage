using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryStorage.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_Pallets_PalletId",
                table: "Boxes");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_Pallets_PalletId",
                table: "Boxes",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boxes_Pallets_PalletId",
                table: "Boxes");

            migrationBuilder.AddForeignKey(
                name: "FK_Boxes_Pallets_PalletId",
                table: "Boxes",
                column: "PalletId",
                principalTable: "Pallets",
                principalColumn: "Id");
        }
    }
}
