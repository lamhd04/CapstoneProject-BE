using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateSupplierv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SupplierAddress",
                table: "Supplier",
                newName: "Province");

            migrationBuilder.AddColumn<string>(
                name: "Block",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Block",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Supplier");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Supplier");

            migrationBuilder.RenameColumn(
                name: "Province",
                table: "Supplier",
                newName: "SupplierAddress");
        }
    }
}
