namespace ParkAssist.API.Models.DTO
{
    public class DiscountDTO
    {
        public required int Id { get; init; }
        public required string Type { get; init; }
        public required string Description { get; init; }
        public required decimal Multiplier { get; init; }
    }
}
