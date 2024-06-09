#nullable disable

namespace StudentSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class Alter_Resources_Table_Remove_Type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Resources");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
