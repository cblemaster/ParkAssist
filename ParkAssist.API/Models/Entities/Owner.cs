namespace ParkAssist.API.Models.Entities;

public partial class Owner
{
    public int OwnerId { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<ParkingLot> ParkingLots { get; set; } = new List<ParkingLot>();

    public virtual User User { get; set; } = null!;
}
