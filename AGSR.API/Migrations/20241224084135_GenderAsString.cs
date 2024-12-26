using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AGSR.TestTask.Migrations
{
    public partial class GenderAsString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "patient",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Patient_Gender",
                table: "patient",
                sql: "Gender IN ('male','female','other','unknown')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Patient_Gender",
                table: "patient");

            migrationBuilder.AlterColumn<int>(
                name: "gender",
                table: "patient",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
