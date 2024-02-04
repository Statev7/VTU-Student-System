using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentSystem.Data.Migrations
{
    public partial class Alter_Student_Table_Add_Applied_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApplied",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApplied",
                table: "Students");
        }
    }
}
