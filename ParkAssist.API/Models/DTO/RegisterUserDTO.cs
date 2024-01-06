namespace ParkAssist.API.Models.DTO
{
    public class RegisterUserDTO
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Phone { get; init; }
    }
}
