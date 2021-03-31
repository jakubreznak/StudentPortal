using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class studentNameAddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "studentName",
                table: "Soubor",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "studentName",
                table: "Hodnoceni",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "studentName",
                table: "Soubor");

            migrationBuilder.DropColumn(
                name: "studentName",
                table: "Hodnoceni");
        }
    }
}
