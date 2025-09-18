using Microlab.web.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Microlab.web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Clinica>  Clinicas  { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Exame>    Exames    { get; set; }
        public DbSet<ResultadoExame> ResultadoExames { get; set; }
        public DbSet<CaractereFisico> CaracteresFisicos { get; set; }
        public DbSet<AnaliseQuimica> AnalisesQuimicas { get; set; }
        public DbSet<Sedimentoscopia> Sedimentoscopias { get; set; }
        public DbSet<Negativo> Negativos { get; set; }
    }
}
