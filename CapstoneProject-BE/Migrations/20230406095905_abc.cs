using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MeasuredUnitId",
                table: "StocktakeNoteDetail",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StocktakeNoteDetail_MeasuredUnitId",
                table: "StocktakeNoteDetail",
                column: "MeasuredUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocktakeNoteDetail_MeasuredUnit_MeasuredUnitId",
                table: "StocktakeNoteDetail",
                column: "MeasuredUnitId",
                principalTable: "MeasuredUnit",
                principalColumn: "MeasuredUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocktakeNoteDetail_MeasuredUnit_MeasuredUnitId",
                table: "StocktakeNoteDetail");

            migrationBuilder.DropIndex(
                name: "IX_StocktakeNoteDetail_MeasuredUnitId",
                table: "StocktakeNoteDetail");

            migrationBuilder.DropColumn(
                name: "MeasuredUnitId",
                table: "StocktakeNoteDetail");
        }
    }
}
