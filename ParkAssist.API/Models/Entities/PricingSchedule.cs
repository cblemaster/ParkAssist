namespace ParkAssist.API.Models.Entities;

public partial class PricingSchedule
{
    public int Id { get; set; }

    public decimal WeekdayRate { get; set; }

    public decimal WeekendRate { get; set; }

    public decimal SpecialEventRate { get; set; }

    public virtual ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();
}
