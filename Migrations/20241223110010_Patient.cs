using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGSR.TestTask.Migrations
{
    public partial class Patient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "patient",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    use = table.Column<string>(type: "text", nullable: true),
                    family = table.Column<string>(type: "text", nullable: false),
                    given_json = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    birth_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patient", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_patient_birth_date",
                table: "patient",
                column: "birth_date");

            migrationBuilder.CreateIndex(
                name: "IX_patient_family",
                table: "patient",
                column: "family");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "patient");
        }
    }
}
