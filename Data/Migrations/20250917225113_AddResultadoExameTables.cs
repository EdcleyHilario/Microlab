using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddResultadoExameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadoExames",
                columns: table => new
                {
                    ResultadoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusResultado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObservacaoGeral = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataResultado = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadoExames", x => x.ResultadoExameId);
                    table.ForeignKey(
                        name: "FK_ResultadoExames_Exames_ExameId",
                        column: x => x.ExameId,
                        principalTable: "Exames",
                        principalColumn: "ExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnalisesQuimicas",
                columns: table => new
                {
                    AnaliseQuimicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultadoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Glicose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Proteinas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Urobilinogenio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nitrito = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorposCetonicos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leococitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sangue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bilirrubina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcidoAscorbico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hemoglobina = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obs_AnaliseQuimica = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalisesQuimicas", x => x.AnaliseQuimicaId);
                    table.ForeignKey(
                        name: "FK_AnalisesQuimicas_ResultadoExames_ResultadoExameId",
                        column: x => x.ResultadoExameId,
                        principalTable: "ResultadoExames",
                        principalColumn: "ResultadoExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CaracteresFisicos",
                columns: table => new
                {
                    CaractereFisicoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultadoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Volume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Aspecto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Densidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Odor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obs_Caraterfisico = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaracteresFisicos", x => x.CaractereFisicoId);
                    table.ForeignKey(
                        name: "FK_CaracteresFisicos_ResultadoExames_ResultadoExameId",
                        column: x => x.ResultadoExameId,
                        principalTable: "ResultadoExames",
                        principalColumn: "ResultadoExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Negativos",
                columns: table => new
                {
                    NegativoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultadoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Protozoarios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Helmintos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LarvasEncontradas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Negativos", x => x.NegativoId);
                    table.ForeignKey(
                        name: "FK_Negativos_ResultadoExames_ResultadoExameId",
                        column: x => x.ResultadoExameId,
                        principalTable: "ResultadoExames",
                        principalColumn: "ResultadoExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sedimentoscopias",
                columns: table => new
                {
                    SedimentoscopiaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResultadoExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CelulasEpiteliais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Leucocitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hemacias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cristais = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bacterias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cilindros = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obs_Sedimentoscopia = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedimentoscopias", x => x.SedimentoscopiaId);
                    table.ForeignKey(
                        name: "FK_Sedimentoscopias_ResultadoExames_ResultadoExameId",
                        column: x => x.ResultadoExameId,
                        principalTable: "ResultadoExames",
                        principalColumn: "ResultadoExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalisesQuimicas_ResultadoExameId",
                table: "AnalisesQuimicas",
                column: "ResultadoExameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaracteresFisicos_ResultadoExameId",
                table: "CaracteresFisicos",
                column: "ResultadoExameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Negativos_ResultadoExameId",
                table: "Negativos",
                column: "ResultadoExameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResultadoExames_ExameId",
                table: "ResultadoExames",
                column: "ExameId");

            migrationBuilder.CreateIndex(
                name: "IX_Sedimentoscopias_ResultadoExameId",
                table: "Sedimentoscopias",
                column: "ResultadoExameId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalisesQuimicas");

            migrationBuilder.DropTable(
                name: "CaracteresFisicos");

            migrationBuilder.DropTable(
                name: "Negativos");

            migrationBuilder.DropTable(
                name: "Sedimentoscopias");

            migrationBuilder.DropTable(
                name: "ResultadoExames");
        }
    }
}
