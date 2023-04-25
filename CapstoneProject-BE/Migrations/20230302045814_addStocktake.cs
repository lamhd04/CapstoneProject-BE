using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addStocktake : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StocktakeNote",
                columns: table => new
                {
                    StocktakeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Canceled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    State = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    StorageId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakeNote", x => x.StocktakeId);
                    table.ForeignKey(
                        name: "FK_StocktakeNote_Storage_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storage",
                        principalColumn: "StorageId");
                    table.ForeignKey(
                        name: "FK_StocktakeNote_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StocktakeNoteDetail",
                columns: table => new
                {
                    DetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StocktakeId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CurrentStock = table.Column<int>(type: "int", nullable: false),
                    ActualStock = table.Column<int>(type: "int", nullable: false),
                    AmountDifferential = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StocktakeNoteDetail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_StocktakeNoteDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StocktakeNoteDetail_StocktakeNote_StocktakeId",
                        column: x => x.StocktakeId,
                        principalTable: "StocktakeNote",
                        principalColumn: "StocktakeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNote_StorageId",
                table: "StocktakeNote",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNote_UserId",
                table: "StocktakeNote",
                column: "UserId");


            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNoteDetail_ProductId",
                table: "StocktakeNoteDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNoteDetail_StocktakeId",
                table: "StocktakeNoteDetail",
                column: "StocktakeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StocktakeNoteDetail");

            migrationBuilder.DropTable(
                name: "StocktakeNote");
        }
    }
}
