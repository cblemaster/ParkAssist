namespace ParkAssist.API.Models.Entities;

public partial class Vehicle
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public string Make { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Year { get; set; } = null!;

    public string LicensePlate { get; set; } = null!;

    public string Color { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<ParkingSlip> ParkingSlips { get; set; } = new List<ParkingSlip>();
}
