namespace ParkAssist.API.Models.DTO
{
    public class ValetDTO
    {
        public required int ValetId { get; init; }
        public required int UserId { get; init; }
        public required int ParkingLotId { get; init; }
        public required string ParkingLotName { get; init; }
        public required string ParkingLotAddress { get; init; }
        public required string ParkingLotCity { get; init; }
        public required string ParkingLotState { get; init; }
        public required string ParkingLotZip { get; init; }
        public required string ValetUsername { get; init; }
        public required string ValetFirstName { get; init; }
        public required string ValetFullName { get; init; }
        public required string ValetEmail { get; init; }
        public required string ValetPhone { get; init; }


    }
}
