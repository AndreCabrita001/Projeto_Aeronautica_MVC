using Microsoft.EntityFrameworkCore.Migrations;

namespace Projeto_Aeronautica_MVC.Migrations
{
    public partial class ImageId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InfantPrice",
                table: "Flights",
                newName: "ChildPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChildPrice",
                table: "Flights",
                newName: "InfantPrice");
        }
    }
}
