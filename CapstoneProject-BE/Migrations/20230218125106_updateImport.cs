using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateImport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImportCode",
                table: "ImportOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrder_SupplierId",
                table: "ImportOrder",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrder_Supplier_SupplierId",
                table: "ImportOrder",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrder_Supplier_SupplierId",
                table: "ImportOrder");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrder_SupplierId",
                table: "ImportOrder");

            migrationBuilder.DropColumn(
                name: "ImportCode",
                table: "ImportOrder");
        }
    }
}
