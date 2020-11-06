using Microsoft.EntityFrameworkCore.Migrations;

namespace JevoGastosCore.Migrations
{
    public partial class _20201001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsAhorro",
                table: "Etiquetas",
                nullable: true);
            migrationBuilder.Sql
                (
                "UPDATE Etiquetas SET EsAhorro='1' " +
                "WHERE Etiquetas.Tipo = 'Credito'; "
                );
            migrationBuilder.Sql
                (
                "UPDATE Etiquetas SET EsAhorro='0' " +
                "WHERE Etiquetas.Tipo = 'Cuenta'; "
                );
            migrationBuilder.Sql
                (
                "UPDATE Etiquetas SET Tipo='Cuenta' "+
                "WHERE Etiquetas.Tipo = 'Credito'; "
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql
                (
                "UPDATE Etiquetas SET Tipo='Credito' " +
                "WHERE Etiquetas.EsAhorro = '1' AND Etiquetas.Tipo='Cuenta'; "
                );
            migrationBuilder.DropColumn(
                name: "EsAhorro",
                table: "Etiquetas");
        }
    }
}
