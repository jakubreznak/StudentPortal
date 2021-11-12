using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class HodnoceniLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HodnoceniLikes",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "integer", nullable: false),
                    HodnoceniId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HodnoceniLikes", x => new { x.StudentId, x.HodnoceniId });
                    table.ForeignKey(
                        name: "FK_HodnoceniLikes_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HodnoceniLikes_Hodnoceni_HodnoceniId",
                        column: x => x.HodnoceniId,
                        principalTable: "Hodnoceni",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HodnoceniLikes_HodnoceniId",
                table: "HodnoceniLikes",
                column: "HodnoceniId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HodnoceniLikes");
        }
    }
}
