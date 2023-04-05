using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateReturnsv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReturnsCode",
                table: "ReturnsOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "ReturnsOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrder_StorageId",
                table: "ReturnsOrder",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnsOrder_Storage_StorageId",
                table: "ReturnsOrder",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnsOrder_Storage_StorageId",
                table: "ReturnsOrder");

            migrationBuilder.DropIndex(
                name: "IX_ReturnsOrder_StorageId",
                table: "ReturnsOrder");

            migrationBuilder.DropColumn(
                name: "ReturnsCode",
                table: "ReturnsOrder");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "ReturnsOrder");
        }
    }
}
