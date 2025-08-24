using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class TB_Exame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exames",
                columns: table => new
                {
                    ExameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NmExame = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Metodo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDigitacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Digitador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Observacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PacienteId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exames", x => x.ExameId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exames");
        }
    }
}
