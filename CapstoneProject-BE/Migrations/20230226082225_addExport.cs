using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addExport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ImportOrderDetail");

            migrationBuilder.DropColumn(
                name: "Discount",
                table: "ImportOrder");

            migrationBuilder.RenameColumn(
                name: "ActionType",
                table: "ProductHistory",
                newName: "ActionId");

            migrationBuilder.CreateTable(
                name: "ActionType",
                columns: table => new
                {
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionType", x => x.ActionId);
                });

            migrationBuilder.CreateTable(
                name: "ExportOrder",
                columns: table => new
                {
                    ExportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExportCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<float>(type: "real", nullable: false),
                    TotalCost = table.Column<float>(type: "real", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    Paid = table.Column<float>(type: "real", nullable: false),
                    InDebted = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    OtherExpense = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Completed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Denied = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportOrder", x => x.ExportId);
                    table.ForeignKey(
                        name: "FK_ExportOrder_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Supplier",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportOrder_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExportOrderDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExportId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MeasuredUnitId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Price = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExportOrderDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_ExportOrderDetail_ExportOrder_ExportId",
                        column: x => x.ExportId,
                        principalTable: "ExportOrder",
                        principalColumn: "ExportId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExportOrderDetail_MeasuredUnit_MeasuredUnitId",
                        column: x => x.MeasuredUnitId,
                        principalTable: "MeasuredUnit",
                        principalColumn: "MeasuredUnitId");
                    table.ForeignKey(
                        name: "FK_ExportOrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductHistory_ActionId",
                table: "ProductHistory",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrder_SupplierId",
                table: "ExportOrder",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrder_UserId",
                table: "ExportOrder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetail_ExportId",
                table: "ExportOrderDetail",
                column: "ExportId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetail_MeasuredUnitId",
                table: "ExportOrderDetail",
                column: "MeasuredUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExportOrderDetail_ProductId",
                table: "ExportOrderDetail",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductHistory_ActionType_ActionId",
                table: "ProductHistory",
                column: "ActionId",
                principalTable: "ActionType",
                principalColumn: "ActionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductHistory_ActionType_ActionId",
                table: "ProductHistory");

            migrationBuilder.DropTable(
                name: "ActionType");

            migrationBuilder.DropTable(
                name: "ExportOrderDetail");

            migrationBuilder.DropTable(
                name: "ExportOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductHistory_ActionId",
                table: "ProductHistory");

            migrationBuilder.RenameColumn(
                name: "ActionId",
                table: "ProductHistory",
                newName: "ActionType");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "ImportOrderDetail",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "ImportOrder",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
