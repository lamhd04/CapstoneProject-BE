using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateReturnsv1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ReturnsOrder",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "ReturnsOrder");
        }
    }
}
