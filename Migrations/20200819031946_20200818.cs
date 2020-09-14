using Microsoft.EntityFrameworkCore.Migrations;

namespace JevoGastosCore.Migrations
{
    public partial class _20200818 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsMesFijo",
                table: "Planes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsMesFijo",
                table: "Planes");
        }
    }
}
