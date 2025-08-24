using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoFluxo_Solicitacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicaId",
                table: "Pacientes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "PacienteId",
                table: "Exames",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Clinicas",
                columns: table => new
                {
                    ClinicaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clinicas", x => x.ClinicaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_ClinicaId",
                table: "Pacientes",
                column: "ClinicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Exames_PacienteId",
                table: "Exames",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exames_Pacientes_PacienteId",
                table: "Exames",
                column: "PacienteId",
                principalTable: "Pacientes",
                principalColumn: "PacienteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Clinicas_ClinicaId",
                table: "Pacientes",
                column: "ClinicaId",
                principalTable: "Clinicas",
                principalColumn: "ClinicaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exames_Pacientes_PacienteId",
                table: "Exames");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Clinicas_ClinicaId",
                table: "Pacientes");

            migrationBuilder.DropTable(
                name: "Clinicas");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_ClinicaId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Exames_PacienteId",
                table: "Exames");

            migrationBuilder.DropColumn(
                name: "ClinicaId",
                table: "Pacientes");

            migrationBuilder.AlterColumn<string>(
                name: "PacienteId",
                table: "Exames",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
