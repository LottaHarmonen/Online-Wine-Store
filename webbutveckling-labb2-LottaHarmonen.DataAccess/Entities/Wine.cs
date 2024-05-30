using System.ComponentModel.DataAnnotations;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

public class Wine
{
    [Key]
    public int ProductId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public int TypeId { get; set; }

    public bool Status { get; set; }


}