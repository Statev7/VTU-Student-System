#nullable disable

namespace StudentSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Alter_Course_Table_Add_ImageFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFolder",
                table: "Courses",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFolder",
                table: "Courses");
        }
    }
}
