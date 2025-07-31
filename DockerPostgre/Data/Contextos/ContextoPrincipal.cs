using DockerPostgre.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DockerPostgre.Data.Contextos
{
    public class ContextoPrincipal : DbContext
    {
        public ContextoPrincipal(DbContextOptions<ContextoPrincipal> options) : base(options)
        {
    
        }

     
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);

            configurationBuilder.Conventions.Remove(typeof(CascadeDeleteConvention));
            configurationBuilder.Properties<DateTime>().HaveColumnType("timestamp without time zone");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.UseSerialColumns();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoPrincipal).Assembly);
        }


    }
}
