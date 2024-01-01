using Microsoft.EntityFrameworkCore;
using ParkAssist.API.Models.Entities;

namespace ParkAssist.API.Context
{
    public partial class ParkAssistContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => entity.Ignore(user => user.FullName));
            modelBuilder.Entity<ParkingSlip>(entity => entity.Ignore(slip => slip.ParkingStatus));

            modelBuilder.Entity<Customer>(entity => entity.Navigation<User>(nav => nav.User).AutoInclude());
            modelBuilder.Entity<Owner>(entity => entity.Navigation<User>(nav => nav.User).AutoInclude());
            modelBuilder.Entity<Valet>(entity => entity.Navigation<User>(nav => nav.User).AutoInclude());
            modelBuilder.Entity<Valet>(entity => entity.Navigation<ParkingLot>(nav => nav.ParkingLot).AutoInclude());
            modelBuilder.Entity<ParkingLot>(entity => entity.Navigation<Owner>(nav => nav.Owner).AutoInclude());
            modelBuilder.Entity<ParkingLot>(entity => entity.Navigation<PricingSchedule>(nav => nav.PricingSchedule).AutoInclude());
            modelBuilder.Entity<ParkingSpot>(entity => entity.Navigation<ParkingLot>(nav => nav.ParkingLot).AutoInclude());
            modelBuilder.Entity<Vehicle>(entity => entity.Navigation<Customer>(nav => nav.Customer).AutoInclude());
            modelBuilder.Entity<ParkingSlip>(entity => entity.Navigation<ParkingSpot>(nav => nav.ParkingSpot).AutoInclude());
            modelBuilder.Entity<ParkingSlip>(entity => entity.Navigation<Valet>(nav => nav.Valet).AutoInclude());
            modelBuilder.Entity<ParkingSlip>(entity => entity.Navigation<Vehicle>(nav => nav.Vehicle).AutoInclude());
        }
    }
}
