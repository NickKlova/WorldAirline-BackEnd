using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WADatabase.Models.DB_Request;

#nullable disable

namespace WADatabase.Administration.Clients
{
    public partial class WorldAirlinesContext : DbContext
    {
        public WorldAirlinesContext()
        {
        }

        public WorldAirlinesContext(DbContextOptions<WorldAirlinesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Airport> Airports { get; set; }
        public virtual DbSet<Crew> Crews { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<Pilot> Pilots { get; set; }
        public virtual DbSet<Plane> Planes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketScheme> TicketSchemes { get; set; }
        public virtual DbSet<TravelClass> TravelClasses { get; set; }
        public virtual DbSet<Way> Ways { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Balance)
                    .HasColumnType("money")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Phone).HasMaxLength(100);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_RoleId$Accounts_Roles");
            });

            modelBuilder.Entity<Airport>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Airports)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LocationId$Airports_Locations");
            });

            modelBuilder.Entity<Crew>(entity =>
            {
                entity.HasOne(d => d.CrewPosition)
                    .WithMany(p => p.Crews)
                    .HasForeignKey(d => d.CrewPositionId)
                    .HasConstraintName("FK_CrewPositionId$Crews_Positions");

                entity.HasOne(d => d.Pilot)
                    .WithMany(p => p.Crews)
                    .HasForeignKey(d => d.PilotId)
                    .HasConstraintName("FK_PilotId$Crews_Pilots");

                entity.HasOne(d => d.TicketScheme)
                    .WithMany(p => p.Crews)
                    .HasForeignKey(d => d.TicketSchemeId)
                    .HasConstraintName("FK_TicketSchemeId$Crews_Tickethemes");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasIndex(e => e.PassportSeries, "UQ_PassportSeries")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PassportSeries)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Pilot>(entity =>
            {
                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Pilots)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AccountId$Pilots_Accounts");
            });

            modelBuilder.Entity<Plane>(entity =>
            {
                entity.Property(e => e.ManufactureDate).HasColumnType("date");

                entity.Property(e => e.Model)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.PositionName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_AccountId$Tickets_Accounts");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("FK_PassengerId$Tickets_Passengers");

                entity.HasOne(d => d.TicketScheme)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TicketSchemeId)
                    .HasConstraintName("FK_TicketSchemeId$Tickets_TicketShemes");

                entity.HasOne(d => d.TravelClass)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TravelClassId)
                    .HasConstraintName("FK_TravelClassId$Tickets_TravelClasses");
            });

            modelBuilder.Entity<TicketScheme>(entity =>
            {
                entity.HasIndex(e => new { e.PlaneId, e.DepartureDate, e.ArrivalDate }, "UQ_PlaneId$DepartureDate$ArrivalDate")
                    .IsUnique();

                entity.Property(e => e.ArrivalDate).HasColumnType("datetime");

                entity.Property(e => e.DepartureDate).HasColumnType("datetime");

                entity.HasOne(d => d.Plane)
                    .WithMany(p => p.TicketSchemes)
                    .HasForeignKey(d => d.PlaneId)
                    .HasConstraintName("FK_PlaneId$TicketSchemes_Planes");

                entity.HasOne(d => d.Way)
                    .WithMany(p => p.TicketSchemes)
                    .HasForeignKey(d => d.WayId)
                    .HasConstraintName("FK_WayId$TicketSchemes_Ways");
            });

            modelBuilder.Entity<TravelClass>(entity =>
            {
                entity.Property(e => e.ClassName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Way>(entity =>
            {
                entity.HasIndex(e => new { e.DepartureAirportId, e.ArrivalAirportId }, "UQ_DepartureAirportId$ArrivalAirportId")
                    .IsUnique();

                entity.HasOne(d => d.ArrivalAirport)
                    .WithMany(p => p.WayArrivalAirports)
                    .HasForeignKey(d => d.ArrivalAirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArrivalAirportId$Ways_Airports");

                entity.HasOne(d => d.DepartureAirport)
                    .WithMany(p => p.WayDepartureAirports)
                    .HasForeignKey(d => d.DepartureAirportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DepartureAirportId$Ways_Airports");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
