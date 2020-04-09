using Microsoft.EntityFrameworkCore.Migrations;

namespace IssueTracker.Data.Migrations
{
    public partial class ProjectUserJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_ApplicationUserId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_ApplicationUserId",
                table: "ProjectUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ProjectUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId",
                table: "ProjectUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_UserId",
                table: "ProjectUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ProjectUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ProjectUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ApplicationUserId",
                table: "ProjectUsers",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectUsers_AspNetUsers_ApplicationUserId",
                table: "ProjectUsers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
