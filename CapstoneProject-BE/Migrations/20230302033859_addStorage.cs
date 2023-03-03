using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "ImportOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "ExportOrder",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StorageId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Storage",
                columns: table => new
                {
                    StorageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorageName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storage", x => x.StorageId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_StorageId",
                table: "User",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplier_StorageId",
                table: "Supplier",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_StorageId",
                table: "Product",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrder_StorageId",
                table: "ImportOrder",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrder_StorageId",
                table: "ExportOrder",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Category_StorageId",
                table: "Category",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Storage_StorageId",
                table: "Category",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExportOrder_Storage_StorageId",
                table: "ExportOrder",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrder_Storage_StorageId",
                table: "ImportOrder",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Storage_StorageId",
                table: "Product",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_Storage_StorageId",
                table: "Supplier",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Storage_StorageId",
                table: "User",
                column: "StorageId",
                principalTable: "Storage",
                principalColumn: "StorageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Storage_StorageId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_ExportOrder_Storage_StorageId",
                table: "ExportOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrder_Storage_StorageId",
                table: "ImportOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Storage_StorageId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_Storage_StorageId",
                table: "Supplier");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Storage_StorageId",
                table: "User");

            migrationBuilder.DropTable(
                name: "Storage");

            migrationBuilder.DropIndex(
                name: "IX_User_StorageId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Supplier_StorageId",
                table: "Supplier");

            migrationBuilder.DropIndex(
                name: "IX_Product_StorageId",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrder_StorageId",
                table: "ImportOrder");

            migrationBuilder.DropIndex(
                name: "IX_ExportOrder_StorageId",
                table: "ExportOrder");

            migrationBuilder.DropIndex(
                name: "IX_Category_StorageId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "ImportOrder");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "ExportOrder");

            migrationBuilder.DropColumn(
                name: "StorageId",
                table: "Category");
        }
    }
}
