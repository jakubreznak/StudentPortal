using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class Hodnoceni : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hodnoceni",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    text = table.Column<string>(type: "TEXT", nullable: true),
                    rating = table.Column<int>(type: "INTEGER", nullable: false),
                    created = table.Column<string>(type: "TEXT", nullable: true),
                    predmetID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hodnoceni", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Hodnoceni_Predmets_predmetID",
                        column: x => x.predmetID,
                        principalTable: "Predmets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hodnoceni_predmetID",
                table: "Hodnoceni",
                column: "predmetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hodnoceni");
        }
    }
}
