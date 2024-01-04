namespace ParkAssist.API.Models.DTO
{
    public class OwnerDTO
    {
        public required int OwnerId { get; init; }
        public required int UserId { get; init; }
        public required UserDTO User { get; init; }
        public IEnumerable<ParkingLotDTO>? ParkingLots { get; init;}
    }
}
