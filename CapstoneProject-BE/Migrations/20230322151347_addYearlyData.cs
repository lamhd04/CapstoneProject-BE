using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class addYearlyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetail_MeasuredUnit_MeasuredUnitId",
                table: "ImportOrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "ReturnsOrder",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MeasuredUnitId",
                table: "ImportOrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MeasuredUnitId",
                table: "ExportOrderDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "YearlyData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Profit = table.Column<float>(type: "real", nullable: false),
                    InventoryValue = table.Column<float>(type: "real", nullable: false),
                    StorageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyData_Storage_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storage",
                        principalColumn: "StorageId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_YearlyData_StorageId",
                table: "YearlyData",
                column: "StorageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetail_MeasuredUnit_MeasuredUnitId",
                table: "ImportOrderDetail",
                column: "MeasuredUnitId",
                principalTable: "MeasuredUnit",
                principalColumn: "MeasuredUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportOrderDetail_MeasuredUnit_MeasuredUnitId",
                table: "ImportOrderDetail");

            migrationBuilder.DropTable(
                name: "YearlyData");

            migrationBuilder.AlterColumn<int>(
                name: "SupplierId",
                table: "ReturnsOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeasuredUnitId",
                table: "ImportOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MeasuredUnitId",
                table: "ExportOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportOrderDetail_MeasuredUnit_MeasuredUnitId",
                table: "ImportOrderDetail",
                column: "MeasuredUnitId",
                principalTable: "MeasuredUnit",
                principalColumn: "MeasuredUnitId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
