using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.DTOs
{
    public class OwnerDTO : UserBaseDTO
    {
        public required int OwnerId { get; init; }

        public ReadOnlyCollection<ParkingLot>? ParkingLots { get; init; }
    }
}