namespace ParkAssist.API.Models.DTO
{
    public class ValetDTO
    {
        public required int ValetId { get; init; }
        public required int UserId { get; init; }
        public required int ParkingLotId { get; init; }
        public required UserDTO User { get; init; }
    }
}
