using System.ComponentModel.DataAnnotations.Schema;

namespace webbutveckling_labb2_LottaHarmonen.Shared.DTOs;

public class NewOrderDTO
{
    public DateOnly OrderDate { get; set; }

    public int CustomerId { get; set; }

    public virtual ICollection<int> ProductId { get; set; }
}