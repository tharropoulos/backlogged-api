using Microsoft.EntityFrameworkCore;
using backlogged_api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace backlogged_api.Data
{
    // Add Identity Db Context in order to use ASP.NET Identity
    public class BackloggedDBContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
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
            // Base model creation for scaffolded IdentityUser model
            base.OnModelCreating(modelBuilder);
            //specify the relation

            //Franchise-Game one-to-many relationship
            modelBuilder
                .Entity<Franchise>()
                .HasMany(f => f.Games)
                .WithOne(g => g.Franchise)
                .HasForeignKey(g => g.FranchiseId)
                .HasPrincipalKey(e => e.Id);
            //Publisher-Game one-to-many relationship
            modelBuilder
                .Entity<Publisher>()
                .HasMany(f => f.Games)
                .WithOne(g => g.Publisher)
                .HasForeignKey(g => g.PublisherId)
                .HasPrincipalKey(e => e.Id);
            //Game-Review one-to-many relationship 
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.game)
                .HasForeignKey(e => e.GameId)
                .HasPrincipalKey(e => e.Id);
            //User-Review one-to-many relationship
            modelBuilder
                .Entity<User>()
                .HasMany(e => e.Reviews)
                .WithOne(e => e.Author)
                .HasForeignKey(e => e.AuthorId)
                .HasPrincipalKey(e => e.Id);
            // User-Backlog one-to-one relationship
            modelBuilder
                .Entity<User>()
                .HasOne(e => e.Backlog)
                .WithOne(e => e.User)
                .HasForeignKey<Backlog>(e => e.UserId)
                .IsRequired(false);
            // Game-Genre many-to-many relationship
            modelBuilder
                .Entity<Genre>()
                .HasMany(e => e.Games)
                .WithMany(e => e.Genres);
            // Game-Developer many-to-many relationship
            modelBuilder
                .Entity<Developer>()
                .HasMany(e => e.Games)
                .WithMany(e => e.Developers)
                .UsingEntity("GameDeveloper");
            // Game-Platform many-to-many relationship
            modelBuilder
                .Entity<Platform>()
                .HasMany(e => e.Games)
                .WithMany(e => e.Platforms);
            // Game-Backlog many-to-many relationship
            modelBuilder
                .Entity<Game>()
                .HasMany(e => e.Backlogs)
                .WithMany(e => e.Games);

            //generate guids on the database
            modelBuilder
                .Entity<Game>()
                .Property(e => e.FranchiseId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Game>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Game>()
                .Property(e => e.PublisherId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Franchise>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Publisher>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Genre>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Developer>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Platform>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Review>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Review>()
                .Property(e => e.AuthorId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Backlog>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<Backlog>()
                .Property(e => e.UserId)
                .HasDefaultValueSql("gen_random_uuid()");
            modelBuilder
                .Entity<User>()
                .Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Set the email as an index and make it unique
            modelBuilder
                .Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique(true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        public DbSet<Publisher> Publishers { get; set; } = default!;
        public DbSet<Developer> Developers { get; set; } = default!;
        public DbSet<Platform> Platforms { get; set; } = default!;
        public DbSet<Genre> Genres { get; set; } = default!;
        public DbSet<Review> Reviews { get; set; } = default!;
        public DbSet<Backlog> Backlogs { get; set; } = default!;
    }
}
