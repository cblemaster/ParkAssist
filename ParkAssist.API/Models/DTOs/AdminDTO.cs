namespace ParkAssist.API.Models.DTOs
{
    public class AdminDTO : UserBaseDTO
    {
        public required int AdminId { get; init; }

        public static readonly AdminDTO AdminDTONotFound =
            new()
            {
                AdminId = 0,
                UserId = 0,
                Username = "not found",
                FirstName = "not found",
                LastName = "not found",
                FullName = "not found",
                Email = "not found",
                Phone = "not found",
                CreateDate = DateTime.MinValue,
                UpdateDate = null,
            };
    }
}
