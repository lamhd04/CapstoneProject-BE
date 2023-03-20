using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addAvailable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableForReturns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportId = table.Column<int>(type: "int", nullable: true),
                    ExportId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Available = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableForReturns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailableForReturns_ExportOrder_ExportId",
                        column: x => x.ExportId,
                        principalTable: "ExportOrder",
                        principalColumn: "ExportId");
                    table.ForeignKey(
                        name: "FK_AvailableForReturns_ImportOrder_ImportId",
                        column: x => x.ImportId,
                        principalTable: "ImportOrder",
                        principalColumn: "ImportId");
                    table.ForeignKey(
                        name: "FK_AvailableForReturns_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailableForReturns_ExportId",
                table: "AvailableForReturns",
                column: "ExportId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableForReturns_ImportId",
                table: "AvailableForReturns",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_AvailableForReturns_ProductId",
                table: "AvailableForReturns",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableForReturns");
        }
    }
}
