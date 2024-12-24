using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGSR.TestTask.Migrations
{
    public partial class GivenAsArray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "given_json",
                table: "patient");

            migrationBuilder.AddColumn<string[]>(
                name: "given",
                table: "patient",
                type: "text[]",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "given",
                table: "patient");

            migrationBuilder.AddColumn<string>(
                name: "given_json",
                table: "patient",
                type: "text",
                nullable: true);
        }
    }
}
