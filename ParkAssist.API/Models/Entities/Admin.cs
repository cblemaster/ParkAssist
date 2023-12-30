namespace ParkAssist.API.Models.Entities;

public partial class Admin
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
