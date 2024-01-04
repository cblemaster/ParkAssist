using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;

namespace ParkAssist.API.Models.Mappers
{
    public class Mappers
    {
        public VehicleDTO? MapVehicle<T>(Vehicle vehicle) where T : Vehicle => vehicle == null ? null : MapVehicleEntityToVehicleDTO(vehicle);

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

        public ValetDTO? MapValet<T>(Valet valet) where T : Valet => valet == null ? null : MapValetEntityToValetDTO(valet);

        public IEnumerable<ValetDTO>? MapValets<T>(IEnumerable<Valet> valets) where T : IEnumerable<Valet>
        {
            if (valets == null) { return null; }

            List<ValetDTO> dtoList = [];
            foreach (Valet valet in valets)
            {
                dtoList.Add(MapValetEntityToValetDTO(valet));
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

        private static ValetDTO MapValetEntityToValetDTO(Valet valet) => new()
        {
            ValetId = valet.ValetId,
            UserId = valet.UserId,
            ParkingLotId = valet.ParkingLotId,
            ParkingLotName = valet.ParkingLot.Name,
            ParkingLotAddress = valet.ParkingLot.Address,
            ParkingLotCity = valet.ParkingLot.City,
            ParkingLotState = valet.ParkingLot.State,
            ParkingLotZip = valet.ParkingLot.Zip,
            ValetUsername = valet.User.Username,
            ValetFirstName = valet.User.FirstName,
            ValetFullName = valet.User.FullName,
            ValetEmail = valet.User.Email,
            ValetPhone = valet.User.Phone,
        };
    }
}
