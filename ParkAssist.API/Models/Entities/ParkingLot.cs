namespace ParkAssist.API.Models.Entities;

public partial class ParkingLot
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int PricingScheduleId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<ParkingSpot> ParkingSpots { get; set; } = new List<ParkingSpot>();

    public virtual PricingSchedule PricingSchedule { get; set; } = null!;

    public virtual ICollection<Valet> Valets { get; set; } = new List<Valet>();
}
