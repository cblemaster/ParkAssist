using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;

namespace ParkAssist.API.Models.Mappers
{
    public class Mappers
    {
        public VehicleDTO? MapVehicle<T>(Vehicle vehicle) where T : Vehicle
        {
            if (vehicle == null) { return null; }
            
            return MapVehicleEntityToVehicleDTO(vehicle);
        }

        public IEnumerable<VehicleDTO>? MapVehicles<T>(IEnumerable<Vehicle> vehicles) where T : IEnumerable<Vehicle>
        {
            if (vehicles == null) { return null; }
            
            List<VehicleDTO> dtoList = [];
            foreach (Vehicle vehicle in vehicles)
            {
                dtoList.Add(MapVehicleEntityToVehicleDTO(vehicle));
            }
            return dtoList.AsReadOnly();
        }

        private static VehicleDTO MapVehicleEntityToVehicleDTO(Vehicle vehicle) => new()
        {
            Id = vehicle.Id,
            UserId = vehicle.Customer.UserId,
            CustomerId = vehicle.Customer.CustomerId,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Color = vehicle.Color,
            Year = vehicle.Year,
            LicensePlate = vehicle.LicensePlate,
            StateLicensedIn = vehicle.StateLicensedIn,
            CreateDate = vehicle.CreateDate,
            UpdateDate = vehicle.UpdateDate,
            CustomerUsername = vehicle.Customer.User.Username,
            CustomerFirstName = vehicle.Customer.User.FirstName,
            CustomerFullName = vehicle.Customer.User.FullName,
            CustomerEmail = vehicle.Customer.User.Email,
            CustomerPhone = vehicle.Customer.User.Phone,
        };
    }
}
