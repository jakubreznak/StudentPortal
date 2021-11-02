using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class StudentDoubleBinding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predmets_AspNetUsers_StudentId",
                table: "Predmets");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Predmets",
                newName: "studentId");

            migrationBuilder.RenameIndex(
                name: "IX_Predmets_StudentId",
                table: "Predmets",
                newName: "IX_Predmets_studentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predmets_AspNetUsers_studentId",
                table: "Predmets",
                column: "studentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predmets_AspNetUsers_studentId",
                table: "Predmets");

            migrationBuilder.RenameColumn(
                name: "studentId",
                table: "Predmets",
                newName: "StudentId");

            migrationBuilder.RenameIndex(
                name: "IX_Predmets_studentId",
                table: "Predmets",
                newName: "IX_Predmets_StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predmets_AspNetUsers_StudentId",
                table: "Predmets",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
