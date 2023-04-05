using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateImportv7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImportOrderDetail_MeasuredUnitId",
                table: "ImportOrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetail_MeasuredUnitId",
                table: "ImportOrderDetail",
                column: "MeasuredUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ImportOrderDetail_MeasuredUnitId",
                table: "ImportOrderDetail");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetail_MeasuredUnitId",
                table: "ImportOrderDetail",
                column: "MeasuredUnitId",
                unique: true);
        }
    }
}
