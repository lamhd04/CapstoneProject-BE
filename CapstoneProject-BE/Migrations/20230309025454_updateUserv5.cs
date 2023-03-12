using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    public partial class updateUserv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "User");
        }
    }
}
