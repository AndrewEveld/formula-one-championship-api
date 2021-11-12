using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FormulaOne
{
    public partial class formula1APIContext : DbContext
    {
        public formula1APIContext()
        {
        }

        public formula1APIContext(DbContextOptions<formula1APIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<PointSystem> PointSystems { get; set; }
        public virtual DbSet<PositionPoint> PositionPoints { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<RaceResult> RaceResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=formula1API.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("DRIVERS");

                entity.HasIndex(e => e.Id, "IX_DRIVERS_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateOfBirth)
                    .IsRequired()
                    .HasColumnName("DATE_OF_BIRTH");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FIRST_NAME");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME");
            });

            modelBuilder.Entity<PointSystem>(entity =>
            {
                entity.ToTable("POINT_SYSTEMS");

                entity.HasIndex(e => e.Id, "IX_POINT_SYSTEMS_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SeasonEnded).HasColumnName("SEASON_ENDED");

                entity.Property(e => e.SeasonStarted).HasColumnName("SEASON_STARTED");
            });

            modelBuilder.Entity<PositionPoint>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("POSITION_POINTS");

                entity.Property(e => e.PointSystemId).HasColumnName("POINT_SYSTEM_ID");

                entity.Property(e => e.Points).HasColumnName("POINTS");

                entity.Property(e => e.Position).HasColumnName("POSITION");

                entity.HasOne(d => d.PointSystem)
                    .WithMany()
                    .HasForeignKey(d => d.PointSystemId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Race>(entity =>
            {
                entity.ToTable("RACES");

                entity.HasIndex(e => e.Id, "IX_RACES_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Season).HasColumnName("SEASON");
            });

            modelBuilder.Entity<RaceResult>(entity =>
            {
                entity.ToTable("RACE_RESULTS");

                entity.HasIndex(e => e.Id, "IX_RACE_RESULTS_ID")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DriverId).HasColumnName("DRIVER_ID");

                entity.Property(e => e.Position).HasColumnName("POSITION");

                entity.Property(e => e.RaceId).HasColumnName("RACE_ID");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.RaceResults)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Race)
                    .WithMany(p => p.RaceResults)
                    .HasForeignKey(d => d.RaceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
