using System.ComponentModel.DataAnnotations;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

public class Customer
{
    [Key]
    public int Id { get; set; }
    public string FirstName { get; set; }

    public string Surname { get; set;}

    [EmailAddress]
    public string Email { get; set;}

    [Phone]
    public string PhoneNumber { get; set;}

    public string Password { get; set;}

    public bool IsAdmin { get; set; }

    public string Address { get; set;}

    public virtual ICollection<Order> Orders { get; set; }

}

