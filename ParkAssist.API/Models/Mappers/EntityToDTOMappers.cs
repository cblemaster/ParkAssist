using ParkAssist.API.Models.DTO;
using ParkAssist.API.Models.Entities;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models.Mappers
{
    public class EntityToDTOMappers
    {
        public static CustomerDTO? MapCustomer(Customer customer) => customer == null
                ? null
                : new()
                {
                    CustomerId = customer.CustomerId,
                    UserId = customer.UserId,
                    User = MapUser(customer.User)!,
                };

        public static ReadOnlyCollection<CustomerDTO>? MapCustomers(IEnumerable<Customer> customers)
        {
            if (customers == null) { return null; }

            List<CustomerDTO> dtoList = [];
            foreach (Customer customer in customers)
            {
                dtoList.Add(MapCustomer(customer)!);
            }
            return dtoList.AsReadOnly();
        }

        public static DiscountDTO? MapDiscount(Discount discount) => discount == null
                ? null
                : new()
                {
                    Id = discount.Id,
                    Type = discount.Type,
                    Description = discount.Description,
                    Multiplier = discount.Multiplier,
                };

        public static ReadOnlyCollection<DiscountDTO>? MapDiscounts(IEnumerable<Discount> discounts)
        {
            if (discounts == null) { return null; }

            List<DiscountDTO> dtoList = [];
            foreach (Discount discount in discounts)
            {
                dtoList.Add(MapDiscount(discount)!);
            }
            return dtoList.AsReadOnly();
        }

        public static OwnerDTO? MapOwner(Owner owner) => owner == null
                ? null
                : new()
                {
                    OwnerId = owner.OwnerId,
                    UserId = owner.UserId,
                    User = MapUser(owner.User)!,
                };

        public static ReadOnlyCollection<OwnerDTO>? MapOwners(IEnumerable<Owner> owners)
        {
            if (owners == null) { return null; }

            List<OwnerDTO> dtoList = [];
            foreach (Owner owner in owners)
            {
                dtoList.Add(MapOwner(owner)!);
            }
            return dtoList.AsReadOnly();
        }

        public static ParkingLotDTO? MapParkingLot(ParkingLot parkingLot) => parkingLot == null
                ? null
                : new()
                {
                    Id = parkingLot.Id,
                    OwnerId = parkingLot.OwnerId,
                    PricingScheduleId = parkingLot.PricingScheduleId,
                    Name = parkingLot.Name,
                    Address = parkingLot.Address,
                    City = parkingLot.City,
                    State = parkingLot.State,
                    Zip = parkingLot.Zip,
                    CreateDate = parkingLot.CreateDate,
                    UpdateDate = parkingLot.UpdateDate,
                    Owner = MapOwner(parkingLot.Owner)!,
                    PricingSchedule = MapPricingSchedule(parkingLot.PricingSchedule)!,
                    ParkingSpots = MapParkingSpots(parkingLot.ParkingSpots)!,
                    Valets = MapValets(parkingLot.Valets)!,
                };

        public static ReadOnlyCollection<ParkingLotDTO>? MapParkingLots(IEnumerable<ParkingLot> parkingLots)
        {
            if (parkingLots == null) { return null; }

            List<ParkingLotDTO> dtoList = [];
            foreach (ParkingLot parkingLot in parkingLots)
            {
                dtoList.Add(MapParkingLot(parkingLot)!);
            }
            return dtoList.AsReadOnly();
        }

        public static ParkingSlipDTO? MapParkingSlip(ParkingSlip parkingSlip) => parkingSlip == null
                ? null
                : new()
                {
                    Id = parkingSlip.Id,
                    ValetId = parkingSlip.ValetId,
                    VehicleId = parkingSlip.VehicleId,
                    ParkingStatus = parkingSlip.ParkingStatus.ToString(),
                    ParkingSpotId = parkingSlip.ParkingSpotId,
                    TimeIn = parkingSlip.TimeIn,
                    TimeOut = parkingSlip.TimeOut,
                    AmountDue = parkingSlip.AmountDue,
                    AmountPaid = parkingSlip.AmountPaid,
                    CreateDate = parkingSlip.CreateDate,
                    UpdateDate = parkingSlip.UpdateDate,
                    ParkingSpot = MapParkingSpot(parkingSlip.ParkingSpot!),
                    Valet = MapValet(parkingSlip.Valet!),
                    Vehicle = MapVehicle(parkingSlip.Vehicle)!,
                    Discounts = MapDiscounts(parkingSlip.Discounts)!,
                };

        public static ReadOnlyCollection<ParkingSlipDTO>? MapParkingSlips(IEnumerable<ParkingSlip> parkingSlips)
        {
            if (parkingSlips == null) { return null; }

            List<ParkingSlipDTO> dtoList = [];
            foreach (ParkingSlip parkingSlip in parkingSlips)
            {
                dtoList.Add(MapParkingSlip(parkingSlip)!);
            }
            return dtoList.AsReadOnly();
        }

        public static ParkingSpotDTO? MapParkingSpot(ParkingSpot parkingSpot) => parkingSpot == null
                ? null
                : new()
                {
                    Id = parkingSpot.Id,
                    ParkingLotId = parkingSpot.Id,
                    Name = parkingSpot.Name,
                    CreateDate = parkingSpot.CreateDate,
                    UpdateDate = parkingSpot.UpdateDate,
                };

        public static ReadOnlyCollection<ParkingSpotDTO>? MapParkingSpots(IEnumerable<ParkingSpot> parkingSpots)
        {
            if (parkingSpots == null) { return null; }

            List<ParkingSpotDTO> dtoList = [];
            foreach (ParkingSpot parkingSpot in parkingSpots)
            {
                dtoList.Add(MapParkingSpot(parkingSpot)!);
            }
            return dtoList.AsReadOnly();
        }

        public static PricingScheduleDTO? MapPricingSchedule(PricingSchedule pricingSchedule) => pricingSchedule == null
                ? null
                : new()
                {
                    Id = pricingSchedule.Id,
                    WeekdayRate = pricingSchedule.WeekdayRate,
                    WeekendRate = pricingSchedule.WeekendRate,
                    SpecialEventRate = pricingSchedule.SpecialEventRate,
                };

        public static UserDTO? MapUser(User user) => user == null
                ? null
                : new()
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Phone = user.Phone,
                    CreateDate = user.CreateDate,
                    UpdateDate = user.UpdateDate,
                };

        public static ReadOnlyCollection<UserDTO>? MapUsers(IEnumerable<User> users)
        {
            if (users == null) { return null; }

            List<UserDTO> dtoList = [];
            foreach (User user in users)
            {
                dtoList.Add(MapUser(user)!);
            }
            return dtoList.AsReadOnly();
        }

        public static ValetDTO? MapValet(Valet valet) => valet == null
                ? null
                : new()
                {
                    ValetId = valet.ValetId,
                    UserId = valet.UserId,
                    ParkingLotId = valet.ParkingLotId,
                    User = MapUser(valet.User)!,
                };

        public static ReadOnlyCollection<ValetDTO>? MapValets(IEnumerable<Valet> valets)
        {
            if (valets == null) { return null; }

            List<ValetDTO> dtoList = [];
            foreach (Valet valet in valets)
            {
                dtoList.Add(MapValet(valet)!);
            }
            return dtoList.AsReadOnly();
        }

        public static VehicleDTO? MapVehicle(Vehicle vehicle) => vehicle == null
                ? null
                : new()
                {
                    Id = vehicle.Id,
                    CustomerId = vehicle.Customer.CustomerId,
                    Make = vehicle.Make,
                    Model = vehicle.Model,
                    Color = vehicle.Color,
                    Year = vehicle.Year,
                    LicensePlate = vehicle.LicensePlate,
                    StateLicensedIn = vehicle.StateLicensedIn,
                    CreateDate = vehicle.CreateDate,
                    UpdateDate = vehicle.UpdateDate,
                    Customer = MapCustomer(vehicle.Customer)!,
                };

        public static ReadOnlyCollection<VehicleDTO>? MapVehicles(IEnumerable<Vehicle> vehicles)
        {
            if (vehicles == null) { return null; }

            List<VehicleDTO> dtoList = [];
            foreach (Vehicle vehicle in vehicles)
            {
                dtoList.Add(MapVehicle(vehicle)!);
            }
            return dtoList.AsReadOnly();
        }
    }
}
