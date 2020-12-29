using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class predmet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "datumRegistrace",
                table: "Students",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "oborIdno",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "rocnikRegistrace",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Predmets",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    zkratka = table.Column<string>(type: "TEXT", nullable: true),
                    katedra = table.Column<string>(type: "TEXT", nullable: true),
                    nazev = table.Column<string>(type: "TEXT", nullable: true),
                    kreditu = table.Column<int>(type: "INTEGER", nullable: false),
                    rok = table.Column<string>(type: "TEXT", nullable: true),
                    statut = table.Column<string>(type: "TEXT", nullable: true),
                    doporucenyRocnik = table.Column<int>(type: "INTEGER", nullable: false),
                    doporucenySemestr = table.Column<string>(type: "TEXT", nullable: true),
                    vyznamPredmetu = table.Column<string>(type: "TEXT", nullable: true),
                    vyukaLS = table.Column<string>(type: "TEXT", nullable: true),
                    vyukaZS = table.Column<string>(type: "TEXT", nullable: true),
                    rozsah = table.Column<string>(type: "TEXT", nullable: true),
                    typZk = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmets", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Predmets");

            migrationBuilder.DropColumn(
                name: "datumRegistrace",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "oborIdno",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "rocnikRegistrace",
                table: "Students");
        }
    }
}
