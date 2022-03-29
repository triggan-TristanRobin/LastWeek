using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataManager.Migrations
{
    public partial class AddReviewUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserGuid",
                table: "Reviews",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserGuid",
                table: "Reviews",
                column: "UserGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserGuid",
                table: "Reviews",
                column: "UserGuid",
                principalTable: "Users",
                principalColumn: "Guid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserGuid",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserGuid",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserGuid",
                table: "Reviews");
        }
    }
}
