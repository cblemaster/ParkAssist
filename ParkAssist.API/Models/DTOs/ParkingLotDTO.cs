namespace ParkAssist.API.Models.DTOs
{
    public class ParkingLotDTO
    {
        public required int ParkingLotId { get; init; }
        public required int OwnerId { get; init; }
        public required string Name { get; init; }
        public required string Address { get; init; }
        public required string City { get; init; }
        public required string State { get; init; }
        public required string Zip { get; init; }
    }
}
