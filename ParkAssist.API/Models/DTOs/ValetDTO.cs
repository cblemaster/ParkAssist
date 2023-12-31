using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.DTOs
{
    public class ValetDTO : UserBaseDTO
    {
        public required int ValetId { get; init; }

        public ParkingLot? ParkingLot { get; init; }
    }
}