using System.ComponentModel.DataAnnotations.Schema;

namespace webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

public class OrdersByCustomerDTO
{
    public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    public int AmountOfItems { get; set; }
}