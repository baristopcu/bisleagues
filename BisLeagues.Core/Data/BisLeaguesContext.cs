using System;
using BisLeagues.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BisLeagues.Core.Data
{
    public partial class BisLeaguesContext : DbContext
    {
        public BisLeaguesContext()
        {
        }

        public BisLeaguesContext(DbContextOptions<BisLeaguesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamPlayers> TeamPlayerMapping { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<New> News { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BisLeagues;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>(entity =>
            {

                entity.ToTable("Player");

                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Surname).HasMaxLength(50);


            });

            modelBuilder.Entity<Team>(entity =>
            {

                entity.ToTable("Team");

                entity.Property(e => e.CreatedOnUtc).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(250);



            });

            modelBuilder.Entity<TeamPlayers>()
                .HasKey(tp => new { tp.TeamId, tp.PlayerId });


            modelBuilder.Entity<City>()
                .ToTable("City")
                .HasMany(p => p.Counties);

            modelBuilder.Entity<County>()
                .ToTable("County")
                .HasOne(p => p.City)
                .WithMany(b => b.Counties);

            modelBuilder.Entity<Season>()
                .ToTable("Season")
                .HasMany(p => p.Matches);

            modelBuilder.Entity<Match>()
                .ToTable("Match")
                .HasOne(p => p.Season)
                .WithMany(b => b.Matches);

            modelBuilder.Entity<Result>()
                .ToTable("Result")
                .HasOne(p => p.Match);

            modelBuilder.Entity<Score>()
                .ToTable("Score")
               .HasOne(p => p.Result);

            modelBuilder.Entity<New>()
                 .ToTable("New")
                 .HasOne(p => p.Team);

            modelBuilder.Entity<New>()
                .ToTable("New")
                .HasOne(p => p.Match);

            modelBuilder.Entity<New>()
                .ToTable("New")
                .HasOne(p => p.Season);

            modelBuilder.Entity<Photo>()
                .ToTable("Photo");
        }
    }
}
