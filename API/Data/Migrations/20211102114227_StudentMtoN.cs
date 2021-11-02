using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class StudentMtoN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predmets_AspNetUsers_studentId",
                table: "Predmets");

            migrationBuilder.DropIndex(
                name: "IX_Predmets_studentId",
                table: "Predmets");

            migrationBuilder.DropColumn(
                name: "studentId",
                table: "Predmets");

            migrationBuilder.CreateTable(
                name: "PredmetStudent",
                columns: table => new
                {
                    StudentsId = table.Column<int>(type: "integer", nullable: false),
                    predmetyStudentaID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetStudent", x => new { x.StudentsId, x.predmetyStudentaID });
                    table.ForeignKey(
                        name: "FK_PredmetStudent_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredmetStudent_Predmets_predmetyStudentaID",
                        column: x => x.predmetyStudentaID,
                        principalTable: "Predmets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredmetStudent_predmetyStudentaID",
                table: "PredmetStudent",
                column: "predmetyStudentaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredmetStudent");

            migrationBuilder.AddColumn<int>(
                name: "studentId",
                table: "Predmets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predmets_studentId",
                table: "Predmets",
                column: "studentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predmets_AspNetUsers_studentId",
                table: "Predmets",
                column: "studentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
