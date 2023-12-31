using ParkAssist.Core;

namespace ParkAssist.API.Models.DTOs
{
    public class UserBaseDTO
    {
        public required int UserId { get; init; }
        public required string Username { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string FullName { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
        public required DateTime CreateDate { get; init; }
        public required DateTime? UpdateDate { get; init; }        
    }
}
