using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addReturns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnsOrder",
                columns: table => new
                {
                    ReturnsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportId = table.Column<int>(type: "int", nullable: true),
                    ExportId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Meadia = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnsOrder", x => x.ReturnsId);
                    table.ForeignKey(
                        name: "FK_ReturnsOrder_ExportOrder_ExportId",
                        column: x => x.ExportId,
                        principalTable: "ExportOrder",
                        principalColumn: "ExportId");
                    table.ForeignKey(
                        name: "FK_ReturnsOrder_ImportOrder_ImportId",
                        column: x => x.ImportId,
                        principalTable: "ImportOrder",
                        principalColumn: "ImportId");
                    table.ForeignKey(
                        name: "FK_ReturnsOrder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId");
                    table.ForeignKey(
                        name: "FK_ReturnsOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnsOrderDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnsId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MeasuredUnitId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnsOrderDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_ReturnsOrderDetail_MeasuredUnit_MeasuredUnitId",
                        column: x => x.MeasuredUnitId,
                        principalTable: "MeasuredUnit",
                        principalColumn: "MeasuredUnitId");
                    table.ForeignKey(
                        name: "FK_ReturnsOrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnsOrderDetail_ReturnsOrder_ReturnsId",
                        column: x => x.ReturnsId,
                        principalTable: "ReturnsOrder",
                        principalColumn: "ReturnsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrder_ExportId",
                table: "ReturnsOrder",
                column: "ExportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrder_ImportId",
                table: "ReturnsOrder",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrder_SupplierId",
                table: "ReturnsOrder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrder_UserId",
                table: "ReturnsOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrderDetail_MeasuredUnitId",
                table: "ReturnsOrderDetail",
                column: "MeasuredUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrderDetail_ProductId",
                table: "ReturnsOrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnsOrderDetail_ReturnsId",
                table: "ReturnsOrderDetail",
                column: "ReturnsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnsOrderDetail");

            migrationBuilder.DropTable(
                name: "ReturnsOrder");
        }
    }
}
