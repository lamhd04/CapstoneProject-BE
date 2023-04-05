using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportOrder",
                columns: table => new
                {
                    ImportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    OtherExpense = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Paid = table.Column<float>(type: "real", nullable: false),
                    InDebted = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOrder", x => x.ImportId);
                });

            migrationBuilder.CreateTable(
                name: "ImportOrderDetail",
                columns: table => new
                {
                    ImportId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MeasuredUnitId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<float>(type: "real", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportOrderDetail", x => x.ImportId);
                    table.ForeignKey(
                        name: "FK_ImportOrderDetail_ImportOrder_ImportId",
                        column: x => x.ImportId,
                        principalTable: "ImportOrder",
                        principalColumn: "ImportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportOrderDetail_MeasuredUnit_MeasuredUnitId",
                        column: x => x.MeasuredUnitId,
                        principalTable: "MeasuredUnit",
                        principalColumn: "MeasuredUnitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportOrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetail_MeasuredUnitId",
                table: "ImportOrderDetail",
                column: "MeasuredUnitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetail_ProductId",
                table: "ImportOrderDetail",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportOrderDetail");

            migrationBuilder.DropTable(
                name: "ImportOrder");
        }
    }
}
