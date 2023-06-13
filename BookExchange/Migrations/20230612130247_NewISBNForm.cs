using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookExchange.Migrations
{
    public partial class NewISBNForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ISBN",
                table: "Book",
                newName: "ISBN13");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ISBN",
                table: "Book",
                newName: "IX_Book_ISBN13");

            migrationBuilder.AddColumn<string>(
                name: "ISBN10",
                table: "Book",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Book_ISBN10",
                table: "Book",
                column: "ISBN10",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Book_ISBN10",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "ISBN10",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "ISBN13",
                table: "Book",
                newName: "ISBN");

            migrationBuilder.RenameIndex(
                name: "IX_Book_ISBN13",
                table: "Book",
                newName: "IX_Book_ISBN");
        }
    }
}
