using Microsoft.EntityFrameworkCore;
using ParkAssist.API.Models.Entities;

namespace ParkAssist.API.Context;

public partial class ParkAssistContext : DbContext
{
    public ParkAssistContext()
    {
    }

    public ParkAssistContext(DbContextOptions<ParkAssistContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<ParkingLot> ParkingLots { get; set; }

    public virtual DbSet<ParkingSlip> ParkingSlips { get; set; }

    public virtual DbSet<ParkingSpot> ParkingSpots { get; set; }

    public virtual DbSet<PricingSchedule> PricingSchedules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Valet> Valets { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UC_CustomerUserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Customers_Users");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasIndex(e => e.Type, "UC_Type").IsUnique();

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Multiplier).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UC_OwnerUserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Owner)
                .HasForeignKey<Owner>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Owners_Users");
        });

        modelBuilder.Entity<ParkingLot>(entity =>
        {
            entity.HasIndex(e => e.Name, "UC_Name").IsUnique();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.City)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PricingScheduleId).HasColumnName("PricingScheduleID");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Zip)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Owner).WithMany(p => p.ParkingLots)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingLots_Owners");

            entity.HasOne(d => d.PricingSchedule).WithMany(p => p.ParkingLots)
                .HasForeignKey(d => d.PricingScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingLots_PricingSchedules");
        });

        modelBuilder.Entity<ParkingSlip>(entity =>
        {
            entity.Property(e => e.AmountDue).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ParkingSpot).WithMany(p => p.ParkingSlips)
                .HasForeignKey(d => d.ParkingSpotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingSlips_ParkingSpots");

            entity.HasOne(d => d.Valet).WithMany(p => p.ParkingSlips)
                .HasForeignKey(d => d.ValetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingSlips_Valets");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.ParkingSlips)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingSlips_Vehicles");

            entity.HasMany(d => d.Discounts).WithMany(p => p.ParkingSlips)
                .UsingEntity<Dictionary<string, object>>(
                    "ParkingSlipsDiscount",
                    r => r.HasOne<Discount>().WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParkingSlipsDiscounts_Discounts"),
                    l => l.HasOne<ParkingSlip>().WithMany()
                        .HasForeignKey("ParkingSlipId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ParkingSlipsDiscounts_ParkingSlips"),
                    j =>
                    {
                        j.HasKey("ParkingSlipId", "DiscountId");
                        j.ToTable("ParkingSlipsDiscounts");
                    });
        });

        modelBuilder.Entity<ParkingSpot>(entity =>
        {
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.ParkingSpots)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingSpots_ParkingLots");
        });

        modelBuilder.Entity<PricingSchedule>(entity =>
        {
            entity.Property(e => e.SpecialEventRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WeekdayRate).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.WeekendRate).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email, "UC_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UC_Phone").IsUnique();

            entity.HasIndex(e => e.Username, "UC_Username").IsUnique();

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Valet>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UC_ValetUserId").IsUnique();

            entity.HasOne(d => d.ParkingLot).WithMany(p => p.Valets)
                .HasForeignKey(d => d.ParkingLotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Valets_ParkingLots");

            entity.HasOne(d => d.User).WithOne(p => p.Valet)
                .HasForeignKey<Valet>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Valets_Users");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasIndex(e => new { e.LicensePlate, e.StateLicensedIn }, "UC_LicensePlate").IsUnique();

            entity.Property(e => e.Color)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Make)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Model)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Year)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.StateLicensedIn)
                .HasMaxLength(2)
                .IsUnicode(false);

            entity.HasOne(d => d.Customer).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicles_Customers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
