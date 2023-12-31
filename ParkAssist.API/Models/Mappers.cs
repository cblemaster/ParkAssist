using ParkAssist.API.Models.DTOs;
using ParkAssist.API.Models.Entities;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace ParkAssist.API.Models
{
    public class Mappers
    {
        public AdminDTO Map<T>(Admin admin) where T : Admin
        {
            return new AdminDTO()
            {
                AdminId = admin.Id,
                UserId = admin.UserId,
                Username = admin.User.Username,
                FirstName = admin.User.FirstName,
                LastName = admin.User.LastName,
                FullName = admin.User.FullName,
                Email = admin.User.Email,
                Phone = admin.User.Phone,
                CreateDate = admin.User.CreateDate,
                UpdateDate = admin.User.UpdateDate,
            };
        }

        public ValetDTO Map<T>(Valet valet) where T : Valet
        {
            return new ValetDTO()
            {
                ValetId = valet.Id,
                UserId = valet.UserId,
                Username = valet.User.Username,
                FirstName = valet.User.FirstName,
                LastName = valet.User.LastName,
                FullName = valet.User.FullName,
                Email = valet.User.Email,
                Phone = valet.User.Phone,
                CreateDate = valet.User.CreateDate,
                UpdateDate = valet.User.UpdateDate,
                ParkingLot = Map<ParkingLot>(valet.ParkingLot),
            };
        }

        public CustomerDTO Map<T>(Customer customer) where T : Customer
        {
            return new CustomerDTO()
            {
                CustomerId = customer.Id,
                UserId = customer.UserId,
                Username = customer.User.Username,
                FirstName = customer.User.FirstName,
                LastName = customer.User.LastName,
                FullName = customer.User.FullName,
                Email = customer.User.Email,
                Phone = customer.User.Phone,
                CreateDate = customer.User.CreateDate,
                UpdateDate = customer.User.UpdateDate,
                Vehicles = new ReadOnlyCollection<Vehicle>(customer.Vehicles.ToImmutableList()),
            };
        }

        public OwnerDTO Map<T>(Owner owner) where T : Owner
        {
            return new OwnerDTO()
            {
                OwnerId = owner.Id,
                UserId = owner.UserId,
                Username = owner.User.Username,
                FirstName = owner.User.FirstName,
                LastName = owner.User.LastName,
                FullName = owner.User.FullName,
                Email = owner.User.Email,
                Phone = owner.User.Phone,
                CreateDate = owner.User.CreateDate,
                UpdateDate = owner.User.UpdateDate,
                ParkingLots = new ReadOnlyCollection<ParkingLot>(owner.ParkingLots.ToImmutableList()),
            };
        }

        public ParkingLotDTO Map<T>(ParkingLot parkingLot) where T : ParkingLot
        {
            return new ParkingLotDTO()
            {
                ParkingLotId = parkingLot.Id,
                OwnerId = parkingLot.OwnerId,
                Name = parkingLot.Name,
                Address = parkingLot.Address,
                City = parkingLot.City,
                State = parkingLot.State,
                Zip = parkingLot.Zip,
            };
        }
    }
}
