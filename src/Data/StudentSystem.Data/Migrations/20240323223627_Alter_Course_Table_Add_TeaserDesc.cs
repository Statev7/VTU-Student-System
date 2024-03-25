using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSystem.Data.Migrations
{
    public partial class Alter_Course_Table_Add_TeaserDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeaserDescription",
                table: "Courses",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeaserDescription",
                table: "Courses");
        }
    }
}
