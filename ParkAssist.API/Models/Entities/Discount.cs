namespace ParkAssist.API.Models.Entities;

public partial class Discount
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Multiplier { get; set; }

    public virtual ICollection<ParkingSlip> ParkingSlips { get; set; } = new List<ParkingSlip>();
}
