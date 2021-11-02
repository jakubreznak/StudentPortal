using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Data.Migrations
{
    public partial class PredetInfoDrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredmetInfos");

            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "Predmets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Predmets_StudentId",
                table: "Predmets",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Predmets_AspNetUsers_StudentId",
                table: "Predmets",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Predmets_AspNetUsers_StudentId",
                table: "Predmets");

            migrationBuilder.DropIndex(
                name: "IX_Predmets_StudentId",
                table: "Predmets");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Predmets");

            migrationBuilder.CreateTable(
                name: "PredmetInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StudentId = table.Column<int>(type: "integer", nullable: true),
                    katedra = table.Column<string>(type: "text", nullable: true),
                    nazev = table.Column<string>(type: "text", nullable: true),
                    rok = table.Column<string>(type: "text", nullable: true),
                    semestr = table.Column<string>(type: "text", nullable: true),
                    vyukaLS = table.Column<string>(type: "text", nullable: true),
                    vyukaZS = table.Column<string>(type: "text", nullable: true),
                    zkratka = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredmetInfos", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PredmetInfos_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PredmetInfos_StudentId",
                table: "PredmetInfos",
                column: "StudentId");
        }
    }
}
