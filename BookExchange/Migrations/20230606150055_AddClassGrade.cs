using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookExchange.Migrations
{
    public partial class AddClassGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Classes");
        }
    }
}
