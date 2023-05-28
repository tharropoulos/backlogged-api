using Microsoft.EntityFrameworkCore;
using backlogged_api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;

namespace backlogged_api.Data
{
    public class BackloggedDBContext : DbContext
    {
        protected readonly IConfiguration Configuration;
        public BackloggedDBContext(DbContextOptions<BackloggedDBContext> options, IConfiguration configuration) : base(options)
        {
            Configuration = configuration;
        }
        public DbSet<Game> Games { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Game>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }
}
