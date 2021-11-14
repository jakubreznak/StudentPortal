using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class EditedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "edited",
                table: "Replies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "edited",
                table: "Hodnoceni",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "edited",
                table: "Comments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "edited",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "edited",
                table: "Hodnoceni");

            migrationBuilder.DropColumn(
                name: "edited",
                table: "Comments");
        }
    }
}
