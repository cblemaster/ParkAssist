namespace ParkAssist.API.Models.DTO
{
    public class ParkingLotDTO
    {
        public required int Id { get; init; }
        public required int OwnerId { get; init; }
        public required int PricingScheduleId { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string State { get; init; }
        public required string Zip { get; init; }
        public required DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
        public required OwnerDTO Owner { get; init; }
        public required PricingScheduleDTO PricingSchedule { get; init; }
        public required IEnumerable<ParkingSpotDTO> ParkingSpots { get; init; }
        public required IEnumerable<ValetDTO> Valets { get; init; }
    }
}
