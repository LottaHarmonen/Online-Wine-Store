using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

public class OrderWine
{

    [Key]
    public int OrderId { get; set; }

    [Key]
    public int WineId { get; set; }
    public int Quantity { get; set; }
    public virtual Order Order { get; set; } = null!;
    public virtual Wine Wine { get; set; } = null!;



}