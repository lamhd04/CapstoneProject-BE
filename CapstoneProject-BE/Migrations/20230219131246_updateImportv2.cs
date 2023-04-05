using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateImportv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ImportOrder_UserId",
                table: "ImportOrder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrder_User_UserId",
                table: "ImportOrder",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrder_User_UserId",
                table: "ImportOrder");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrder_UserId",
                table: "ImportOrder");
        }
    }
}
