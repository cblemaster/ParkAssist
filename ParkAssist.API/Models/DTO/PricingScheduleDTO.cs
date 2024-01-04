namespace ParkAssist.API.Models.DTO
{
    public class PricingScheduleDTO
    {
        public required int Id { get; init; }
        public required decimal WeekdayRate { get; init; }
        public required decimal WeekendRate { get; init; }
        public required decimal SpecialEventRate { get; init; }
    }
}
