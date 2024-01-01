namespace ParkAssist.API.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Owner? Owner { get; set; }

    public virtual Valet? Valet { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}
