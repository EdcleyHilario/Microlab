using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    PacienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Idade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solicitante = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocolo = table.Column<int>(type: "int", nullable: false),
                    Rg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exame = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataSolicitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SolicitanteId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.PacienteId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
