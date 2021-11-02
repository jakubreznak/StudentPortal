using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace API.Data.Migrations
{
    public partial class PredmetyInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredmetInfos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    katedra = table.Column<string>(type: "text", nullable: true),
                    zkratka = table.Column<string>(type: "text", nullable: true),
                    rok = table.Column<string>(type: "text", nullable: true),
                    nazev = table.Column<string>(type: "text", nullable: true),
                    semestr = table.Column<string>(type: "text", nullable: true),
                    vyukaZS = table.Column<string>(type: "text", nullable: true),
                    vyukaLS = table.Column<string>(type: "text", nullable: true),
                    StudentId = table.Column<int>(type: "integer", nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredmetInfos");
        }
    }
}
