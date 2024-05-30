using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

public class Order
{
    [Key] public int Id { get; set; }

    public DateOnly OrderDate { get; set; }

    [ForeignKey("Customer")] public int CustomerId { get; set; }

    public virtual ICollection<Wine> Wines { get; set; }

}