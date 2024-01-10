using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;

namespace ParkAssist.API.Models.Mappers
{
    public class DTOToEntityMappers
    {
        public static Vehicle? MapVehicle(VehicleDTO vehicle, CustomerDTO customer) => vehicle == null || customer == null
                ? null
                : new Vehicle
                {
                    Id = vehicle.Id,
                    CustomerId = vehicle.CustomerId,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Color = vehicle.Color,
                    Year = vehicle.Year,
                    LicensePlate = vehicle.LicensePlate,
                    StateLicensedIn = vehicle.StateLicensedIn,
                    CreateDate = DateTime.Today,
                    UpdateDate = null,
                    Customer = MapCustomer(customer)!,
                };

        public static Customer? MapCustomer(CustomerDTO customer) => customer == null
                ? null
                : new Customer
                {
                    CustomerId = customer.CustomerId,
                    UserId = customer.UserId,
                    User = MapUser(customer.User)!,
                };

        public static User? MapUser(UserDTO user) => user == null
                ? null
                : new User
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    CreateDate = user.CreateDate,
                    UpdateDate = user.UpdateDate,
                };

        public static User? MapRegisterUser(RegisterUserDTO registerUser)
        {
            if (registerUser == null)
            {
                return null;
            }
            return new()
            {
                Username = registerUser.Username,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                Email = registerUser.Email,
                Phone = registerUser.Phone,
                CreateDate = DateTime.Today,
                UpdateDate = null,
            };
        }
    }
}
