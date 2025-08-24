using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webyeste.Data.Migrations
{
    /// <inheritdoc />
    public partial class tb_paciente_add_count_exames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtdExames",
                table: "Pacientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtdExames",
                table: "Pacientes");
        }
    }
}
