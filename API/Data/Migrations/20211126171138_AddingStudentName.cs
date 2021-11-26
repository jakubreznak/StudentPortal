using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class AddingStudentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "accountName",
                table: "Topics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "accountName",
                table: "Soubor",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "accountName",
                table: "Replies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "accountName",
                table: "Comments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accountName",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "accountName",
                table: "Soubor");

            migrationBuilder.DropColumn(
                name: "accountName",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "accountName",
                table: "Comments");
        }
    }
}
