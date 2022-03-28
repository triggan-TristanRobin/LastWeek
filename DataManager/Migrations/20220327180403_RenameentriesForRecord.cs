using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataManager.Migrations
{
    public partial class RenameentriesForRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReviewGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Choices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChoiceRecord_Selected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Boundaries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected = table.Column<double>(type: "float", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Records_Reviews_ReviewGuid",
                        column: x => x.ReviewGuid,
                        principalTable: "Reviews",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_ReviewGuid",
                table: "Records",
                column: "ReviewGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Reviews");

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Choices = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Boundaries = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeEntry_Selected = table.Column<double>(type: "float", nullable: true),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Entries_Reviews_ReviewGuid",
                        column: x => x.ReviewGuid,
                        principalTable: "Reviews",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_ReviewGuid",
                table: "Entries",
                column: "ReviewGuid");
        }
    }
}
