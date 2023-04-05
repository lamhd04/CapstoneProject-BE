using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductHistory_UserId",
                table: "ProductHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductHistory_User_UserId",
                table: "ProductHistory",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductHistory_User_UserId",
                table: "ProductHistory");

            migrationBuilder.DropIndex(
                name: "IX_ProductHistory_UserId",
                table: "ProductHistory");
        }
    }
}
