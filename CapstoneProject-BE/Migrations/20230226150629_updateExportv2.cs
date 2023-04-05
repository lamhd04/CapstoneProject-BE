using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateExportv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrder_Supplier_SupplierId",
                table: "ExportOrder");

            migrationBuilder.DropIndex(
                name: "IX_ExportOrder_SupplierId",
                table: "ExportOrder");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "ExportOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "ExportOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrder_SupplierId",
                table: "ExportOrder",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrder_Supplier_SupplierId",
                table: "ExportOrder",
                column: "SupplierId",
                principalTable: "Supplier",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
