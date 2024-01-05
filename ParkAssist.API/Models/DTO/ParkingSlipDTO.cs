namespace ParkAssist.API.Models.DTO
{
    public class ParkingSlipDTO
    {
        public required int Id { get; init; }
        public int? ValetId { get; init; }
        public required int VehicleId { get; init; }
        public required string ParkingStatus { get; init; }
        public int? ParkingSpotId { get; init; }
        public required DateTime TimeIn { get; init; }
        public DateTime? TimeOut { get; init; }
        public decimal? AmountDue { get; init; }
        public decimal? AmountPaid { get; init; }
        public required DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
        public ParkingSpotDTO? ParkingSpot { get; init; }
        public ValetDTO? Valet { get; init; }
        public required VehicleDTO Vehicle { get; init; }
        public required IReadOnlyCollection<DiscountDTO> Discounts { get; init; }
    }
}
