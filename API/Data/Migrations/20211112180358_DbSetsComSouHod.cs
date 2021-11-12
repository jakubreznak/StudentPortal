using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Data.Migrations
{
    public partial class DbSetsComSouHod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Topics_topicID",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comment_CommentId",
                table: "CommentLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_topicID",
                table: "Comments",
                newName: "IX_Comments_topicID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Topics_topicID",
                table: "Comments",
                column: "topicID",
                principalTable: "Topics",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentLikes_Comments_CommentId",
                table: "CommentLikes");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Topics_topicID",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_topicID",
                table: "Comment",
                newName: "IX_Comment_topicID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Topics_topicID",
                table: "Comment",
                column: "topicID",
                principalTable: "Topics",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentLikes_Comment_CommentId",
                table: "CommentLikes",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
