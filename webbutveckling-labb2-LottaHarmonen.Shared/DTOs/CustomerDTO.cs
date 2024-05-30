using System.ComponentModel.DataAnnotations;

namespace webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

public class CustomerDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool IsAdmin { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }


}