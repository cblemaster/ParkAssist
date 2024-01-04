namespace ParkAssist.API.Models.DTO
{
    public class ParkingSpotDTO
    {
        public required int Id { get; init; }
        public required int ParkingLotId { get; init; }
        public required string Name { get; init; }
        public required DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
    }
}
