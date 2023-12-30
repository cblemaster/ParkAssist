namespace ParkAssist.API.Models.Entities;

public partial class Valet
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ParkingLotId { get; set; }

    public virtual ParkingLot ParkingLot { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
