using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookExchange.Migrations
{
    public partial class ClassBookID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Book_BookID",
                table: "Classes");

            migrationBuilder.DropIndex(
                name: "IX_Classes_BookID",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "Classes");

            migrationBuilder.CreateTable(
                name: "ClassBook",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassBook", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassBook");

            migrationBuilder.AddColumn<Guid>(
                name: "BookID",
                table: "Classes",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Classes_BookID",
                table: "Classes",
                column: "BookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Book_BookID",
                table: "Classes",
                column: "BookID",
                principalTable: "Book",
                principalColumn: "BookID");
        }
    }
}
