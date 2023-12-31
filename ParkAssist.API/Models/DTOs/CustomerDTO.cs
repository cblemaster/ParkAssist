using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.DTOs
{
    public class CustomerDTO : UserBaseDTO
    {
        public required int CustomerId { get; init; }

        public required ReadOnlyCollection<Vehicle> Vehicles { get; init; }

        public static readonly CustomerDTO CustomerDTONotFound =
            new()
            {
                CustomerId = 0,
                UserId = 0,
                Username = "not found",
                FirstName = "not found",
                LastName = "not found",
                FullName = "not found",
                Email = "not found",
                Phone = "not found",
                CreateDate = DateTime.MinValue,
                UpdateDate = null,
                Vehicles = new List<Vehicle>().AsReadOnly(),
            };
    }
}