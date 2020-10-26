using System;
using BisLeagues.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamPlayers> TeamPlayerMapping { get; set; }
        public virtual DbSet<Season> Seasons { get; set; }
        public virtual DbSet<Match> Matches { get; set; }
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<New> News { get; set; }
        public virtual DbSet<Gallery> Galleries { get; set; }
        public virtual DbSet<GalleryPhotos> GalleryPhotoMapping { get; set; }
        public virtual DbSet<Point> Point { get; set; }
        public virtual DbSet<PointTableRow> PointTableRows { get; set; }
        public virtual DbSet<GoalKingRow> GoalKingRows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BisLeagues;Trusted_Connection=True;");
                optionsBuilder.ConfigureWarnings(w => w.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
                entity.HasOne(e => e.Player);

            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.ToTable("Player");
                entity.Property(e => e.BirthDate).HasColumnType("date");
                entity.HasOne(e => e.User);

            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team");
                entity.Property(e => e.CreatedOnUtc).HasColumnType("date");
                entity.Property(e => e.Name).HasMaxLength(250);

            });

            modelBuilder.Entity<New>(entity =>
            {
                entity.ToTable("New");
                entity.HasOne(p => p.Season);
                entity.HasOne(p => p.Match);
                entity.HasOne(p => p.Team);
                entity.HasOne(p => p.VideoCoverPhoto);
                entity.HasOne(p => p.CoverPhoto);
                entity.HasOne(p => p.Gallery);

            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

            });

            modelBuilder.Entity<TransferRequest>(entity =>
            {
                entity.ToTable("TransferRequest");

            });

            modelBuilder.Entity<Point>(entity =>
            {
                entity.ToTable("Point");
                entity.HasOne(p => p.Season);
                entity.HasOne(p => p.Result);
            });


            modelBuilder.Entity<Result>(entity =>
            {
                entity.ToTable("Result");
                entity.HasOne(p => p.Match);
                entity.HasOne(p => p.PlayerOfTheMatch);
                entity.HasMany(p => p.Scores);

            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.ToTable("Gallery");

            });

            modelBuilder.Entity<GalleryPhotos>(entity =>
            {
                entity.ToTable("Gallery_Photo_Mapping");
                entity.HasKey(tp => new { tp.GalleryId, tp.PhotoId });

            });

            modelBuilder.Entity<TeamPlayers>()
                .ToTable("Team_Player_Mapping")
                .HasKey(tp => new { tp.TeamId, tp.PlayerId });

            modelBuilder.Entity<UsersRoles>()
                .ToTable("User_UserRole_Mapping")
                .HasKey(bc => new { bc.UserId, bc.RoleId });

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


            modelBuilder.Entity<Score>()
                .ToTable("Score")
               .HasOne(p => p.Result);

            modelBuilder.Entity<Photo>()
                .ToTable("Photo");

            modelBuilder.Entity<PointTableRow>()
                .ToTable("PointTableRow")
                .HasOne(p=>p.Season);

            modelBuilder.Entity<GoalKingRow>()
                .ToTable("GoalKingRow")
                .HasOne(p=>p.Season);

        }
    }
}
