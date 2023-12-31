namespace ParkAssist.API.Models.DTOs
{
    public class ValetDTO : UserBaseDTO
    {
        public required int ValetId { get; init; }

        public ParkingLotDTO? ParkingLot { get; init; }

        public static readonly ValetDTO ValetDTONotFound =
            new()
            {
                ValetId = 0,
                UserId = 0,
                Username = "not found",
                FirstName = "not found",
                LastName = "not found",
                FullName = "not found",
                Email = "not found",
                Phone = "not found",
                CreateDate = DateTime.MinValue,
                UpdateDate = null,
                ParkingLot = null,
            };
    }
}