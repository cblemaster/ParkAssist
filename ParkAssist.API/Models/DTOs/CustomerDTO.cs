using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.DTOs
{
    public class CustomerDTO : UserBaseDTO
    {
        public required int CustomerId { get; init; }

        public ReadOnlyCollection<Vehicle>? Vehicles { get; init; }
    }
}