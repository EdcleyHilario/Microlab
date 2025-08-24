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
    }
}
