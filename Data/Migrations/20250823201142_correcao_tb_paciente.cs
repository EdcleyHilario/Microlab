using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class correcao_tb_paciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SolicitanteId",
                table: "Pacientes");

            migrationBuilder.AlterColumn<int>(
                name: "Idade",
                table: "Pacientes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DataNascimento",
                table: "Pacientes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Idade",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DataNascimento",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "SolicitanteId",
                table: "Pacientes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
