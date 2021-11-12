using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class SouborLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SouborLikes",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    SouborId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SouborLikes", x => new { x.StudentId, x.SouborId });
                    table.ForeignKey(
                        name: "FK_SouborLikes_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SouborLikes_Soubor_SouborId",
                        column: x => x.SouborId,
                        principalTable: "Soubor",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SouborLikes_SouborId",
                table: "SouborLikes",
                column: "SouborId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SouborLikes");
        }
    }
}
