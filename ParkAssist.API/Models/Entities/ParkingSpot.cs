namespace ParkAssist.API.Models.Entities;

public partial class ParkingSpot
{
    public int Id { get; set; }

    public int ParkingLotId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ParkingLot ParkingLot { get; set; } = null!;

    public virtual ICollection<ParkingSlip> ParkingSlips { get; set; } = new List<ParkingSlip>();
}
