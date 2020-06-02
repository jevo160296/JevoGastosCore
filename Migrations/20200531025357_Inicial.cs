using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JevoGastosCore.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TiposEtiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposEtiquetas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etiquetas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    TipoEtiquetaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiquetas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etiquetas_TiposEtiquetas_TipoEtiquetaId",
                        column: x => x.TipoEtiquetaId,
                        principalTable: "TiposEtiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TiposTransacciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    TOrigenId = table.Column<int>(nullable: false),
                    TDestinoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposTransacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TiposTransacciones_TiposEtiquetas_TDestinoId",
                        column: x => x.TDestinoId,
                        principalTable: "TiposEtiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TiposTransacciones_TiposEtiquetas_TOrigenId",
                        column: x => x.TOrigenId,
                        principalTable: "TiposEtiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TipoTransaccionId = table.Column<int>(nullable: false),
                    Fecha = table.Column<DateTime>(nullable: false),
                    OrigenId = table.Column<int>(nullable: false),
                    DestinoId = table.Column<int>(nullable: false),
                    Valor = table.Column<double>(nullable: false),
                    Descripcion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Etiquetas_DestinoId",
                        column: x => x.DestinoId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacciones_Etiquetas_OrigenId",
                        column: x => x.OrigenId,
                        principalTable: "Etiquetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transacciones_TiposTransacciones_TipoTransaccionId",
                        column: x => x.TipoTransaccionId,
                        principalTable: "TiposTransacciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etiquetas_TipoEtiquetaId",
                table: "Etiquetas",
                column: "TipoEtiquetaId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposTransacciones_TDestinoId",
                table: "TiposTransacciones",
                column: "TDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposTransacciones_TOrigenId",
                table: "TiposTransacciones",
                column: "TOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_DestinoId",
                table: "Transacciones",
                column: "DestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_OrigenId",
                table: "Transacciones",
                column: "OrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_TipoTransaccionId",
                table: "Transacciones",
                column: "TipoTransaccionId");
            //Generando Tipos
            migrationBuilder.Sql(
                "DELETE FROM 'TiposEtiquetas';" +
                "INSERT INTO 'TiposEtiquetas' (Id,Name) VALUES (1,'Ingreso');" +
                "INSERT INTO 'TiposEtiquetas' (Id,Name) VALUES (2,'Cuenta');" +
                "INSERT INTO 'TiposEtiquetas' (Id,Name) VALUES (3,'Gasto');"
                );
            migrationBuilder.Sql(
                "DELETE FROM 'TiposTransacciones';" +
                "INSERT INTO 'TiposTransacciones' (Id,Name,TOrigenId,TDestinoId) VALUES (1,'Ingreso',1,2);" +
                "INSERT INTO 'TiposTransacciones' (Id,Name,TOrigenId,TDestinoId) VALUES (2,'Movimiento',2,2);" +
                "INSERT INTO 'TiposTransacciones' (Id,Name,TOrigenId,TDestinoId) VALUES (3,'Gasto',2,3);"
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Etiquetas");

            migrationBuilder.DropTable(
                name: "TiposTransacciones");

            migrationBuilder.DropTable(
                name: "TiposEtiquetas");
        }
    }
}
