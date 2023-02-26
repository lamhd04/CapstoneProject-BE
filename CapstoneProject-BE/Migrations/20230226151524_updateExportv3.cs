using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateExportv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InDebted",
                table: "ExportOrder");

            migrationBuilder.DropColumn(
                name: "OtherExpense",
                table: "ExportOrder");

            migrationBuilder.DropColumn(
                name: "Paid",
                table: "ExportOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "InDebted",
                table: "ExportOrder",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "OtherExpense",
                table: "ExportOrder",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Paid",
                table: "ExportOrder",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
