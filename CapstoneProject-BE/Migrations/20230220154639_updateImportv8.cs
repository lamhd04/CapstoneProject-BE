using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateImportv8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportOrderDetail",
                table: "ImportOrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "OrderCode",
                table: "ProductHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProductHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DetailId",
                table: "ImportOrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportOrderDetail",
                table: "ImportOrderDetail",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportOrderDetail_ImportId",
                table: "ImportOrderDetail",
                column: "ImportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ImportOrderDetail",
                table: "ImportOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_ImportOrderDetail_ImportId",
                table: "ImportOrderDetail");

            migrationBuilder.DropColumn(
                name: "OrderCode",
                table: "ProductHistory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductHistory");

            migrationBuilder.DropColumn(
                name: "DetailId",
                table: "ImportOrderDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ImportOrderDetail",
                table: "ImportOrderDetail",
                column: "ImportId");
        }
    }
}
