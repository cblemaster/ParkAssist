namespace ParkAssist.API.Models.Entities;

public partial class ParkingSlip
{
    public int Id { get; set; }

    public int ValetId { get; set; }

    public int VehicleId { get; set; }

    public int ParkingStatusId { get; set; }

    public int ParkingSpotId { get; set; }

    public DateTime TimeIn { get; set; }

    public DateTime? TimeOut { get; set; }

    public decimal? AmountDue { get; set; }

    public decimal? AmountPaid { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual ParkingSpot ParkingSpot { get; set; } = null!;

    public virtual Vehicle Vehicle { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();

    public ParkingStatus ParkingStatus => (ParkingStatus)ParkingStatusId;
}
