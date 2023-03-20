using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateReturnTotal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Total",
                table: "ReturnsOrder",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "ReturnsOrder");
        }
    }
}
