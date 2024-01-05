namespace ParkAssist.API.Models.DTO
{
    public class CustomerDTO
    {
        public required int CustomerId { get; init; }
        public required int UserId { get; init; }
        public required UserDTO User { get; init; }
    }
}
