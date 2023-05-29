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
        public DbSet<Franchise> Franchises { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //specify the relation
            modelBuilder
                .Entity<Franchise>()
                .HasMany(f => f.games)
                .WithOne(g => g.franchise)
                .HasForeignKey(g => g.franchiseId)
                .HasPrincipalKey(e => e.id);
            modelBuilder
                .Entity<Publisher>()
                .HasMany(f => f.games)
                .WithOne(g => g.publisher)
                .HasForeignKey(g => g.publisherId)
                .HasPrincipalKey(e => e.id);
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.reviews)
                .WithOne(e => e.game)
                .HasForeignKey(e => e.gameId)
                .HasPrincipalKey(e => e.id);
            modelBuilder
                .Entity<Genre>()
                .HasMany(e => e.games)
                .WithMany(e => e.genres);
            modelBuilder
                .Entity<Developer>()
                .HasMany(e => e.games)
                .WithMany(e => e.developers);
            modelBuilder
                .Entity<Platform>()
                .HasMany(e => e.games)
                .WithMany(e => e.platforms);
            //generate guids on the database
            modelBuilder
                .Entity<Game>()
                .Property(e => e.franchiseId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Game>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Game>()
                .Property(e => e.publisherId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Franchise>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Publisher>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Genre>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Developer>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Platform>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Review>()
                .Property(e => e.id)
                .HasDefaultValueSql("gen_random_uuid()");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
    }
}
