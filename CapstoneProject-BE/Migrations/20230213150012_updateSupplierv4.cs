using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateSupplierv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Supplier",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "Block",
                table: "Supplier",
                newName: "District");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Supplier");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Supplier",
                newName: "Province");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Supplier",
                newName: "Block");
        }
    }
}
