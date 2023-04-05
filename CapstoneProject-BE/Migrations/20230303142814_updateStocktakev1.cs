using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateStocktakev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocktakeNote_User_UserId",
                table: "StocktakeNote");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StocktakeNote",
                newName: "UpdatedId");

            migrationBuilder.RenameIndex(
                name: "IX_StocktakeNote_UserId",
                table: "StocktakeNote",
                newName: "IX_StocktakeNote_UpdatedId");

            migrationBuilder.AddColumn<string>(
                name: "UserCode",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedId",
                table: "StocktakeNote",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "StocktakeCode",
                table: "StocktakeNote",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNote_CreatedId",
                table: "StocktakeNote",
                column: "CreatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakeNote_User_CreatedId",
                table: "StocktakeNote",
                column: "CreatedId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakeNote_User_UpdatedId",
                table: "StocktakeNote",
                column: "UpdatedId",
                principalTable: "User",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocktakeNote_User_CreatedId",
                table: "StocktakeNote");

            migrationBuilder.DropForeignKey(
                name: "FK_StocktakeNote_User_UpdatedId",
                table: "StocktakeNote");

            migrationBuilder.DropIndex(
                name: "IX_StocktakeNote_CreatedId",
                table: "StocktakeNote");

            migrationBuilder.DropColumn(
                name: "UserCode",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedId",
                table: "StocktakeNote");

            migrationBuilder.DropColumn(
                name: "StocktakeCode",
                table: "StocktakeNote");

            migrationBuilder.RenameColumn(
                name: "UpdatedId",
                table: "StocktakeNote",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StocktakeNote_UpdatedId",
                table: "StocktakeNote",
                newName: "IX_StocktakeNote_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakeNote_User_UserId",
                table: "StocktakeNote",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
