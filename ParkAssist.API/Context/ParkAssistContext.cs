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

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<ParkingLot> ParkingLots { get; set; }

    public virtual DbSet<ParkingSlip> ParkingSlips { get; set; }

    public virtual DbSet<ParkingSpot> ParkingSpots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Valet> Valets { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UC_AdminsUserId").IsUnique();

            entity.HasOne(d => d.User).WithOne(p => p.Admin)
                .HasForeignKey<Admin>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admins_Users");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.UserId, "UC_CustomersUserId").IsUnique();

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
            entity.HasIndex(e => e.UserId, "UC_OwnersUserId").IsUnique();

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
        });

        modelBuilder.Entity<ParkingSlip>(entity =>
        {
            /* Parking slip should have an FK on ValetId to Valet(Id);
               I left this out of the db to simplify the object graph
               and to mitigate potential EF errors when scaffolding the
               db; This does mean that if I need to navigate from parking
               slip to valet (or vice versa), I have to do so manually
               by matching keys
            */

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.Property(e => e.AmountDue).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.AmountPaid).HasColumnType("decimal(13, 2)");
            entity.Property(e => e.TimeIn).HasColumnType("datetime");
            entity.Property(e => e.TimeOut).HasColumnType("datetime");

            entity.HasOne(d => d.ParkingSpot).WithMany(p => p.ParkingSlips)
                .HasForeignKey(d => d.ParkingSpotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ParkingSlips_ParkingSpots");

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
            entity.HasIndex(e => e.LicensePlate, "UC_LicensePlate").IsUnique();

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

            entity.HasOne(d => d.Customer).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vehicles_Customers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
