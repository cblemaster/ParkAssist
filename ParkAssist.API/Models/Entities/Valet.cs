namespace ParkAssist.API.Models.Entities;

public partial class Valet
{
    public int ValetId { get; set; }

    public int UserId { get; set; }

    public int ParkingLotId { get; set; }

    public virtual ParkingLot ParkingLot { get; set; } = null!;

    public virtual ICollection<ParkingSlip> ParkingSlips { get; set; } = new List<ParkingSlip>();

    public virtual User User { get; set; } = null!;
}
