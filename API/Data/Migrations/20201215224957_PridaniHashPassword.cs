using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class PridaniHashPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "password",
                table: "Students");

            migrationBuilder.AddColumn<byte[]>(
                name: "passwordHash",
                table: "Students",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "passwordSalt",
                table: "Students",
                type: "BLOB",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "passwordHash",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "passwordSalt",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Students",
                type: "TEXT",
                nullable: true);
        }
    }
}
