using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.DTOs
{
    public class OwnerDTO : UserBaseDTO
    {
        public required int OwnerId { get; init; }

        public required ReadOnlyCollection<ParkingLot> ParkingLots { get; init; }

        public static readonly OwnerDTO OwnerDTONotFound =
            new()
            {
                OwnerId = 0,
                UserId = 0,
                Username = "not found",
                FirstName = "not found",
                LastName = "not found",
                FullName = "not found",
                Email = "not found",
                Phone = "not found",
                CreateDate = DateTime.MinValue,
                UpdateDate = null,
                ParkingLots = new List<ParkingLot>().AsReadOnly(),
            };
    }
}