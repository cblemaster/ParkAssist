namespace ParkAssist.API.Models.DTO
{
    public class VehicleDTO
    {
        public required int Id { get; init; }
        public required int CustomerId { get; init; }
        public required int UserId { get; init; }
        public required string Make { get; init; }
        public required string Model { get; init; }
        public required string Year { get; init; }
        public required string Color { get; init; }
        public required string LicensePlate { get; init; }
        public required string StateLicensedIn { get; init; }
        public required DateTime CreateDate { get; init; }
        public DateTime? UpdateDate { get; init; }
        public required string CustomerUsername { get; init; }
        public required string CustomerFirstName { get; init; }
        public required string CustomerFullName { get; init; }
        public required string CustomerEmail { get; init; }
        public required string CustomerPhone { get; init; }
    }
}
