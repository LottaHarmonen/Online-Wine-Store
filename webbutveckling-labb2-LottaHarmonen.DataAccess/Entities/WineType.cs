using System.ComponentModel.DataAnnotations;

namespace webbutveckling_labb2_LottaHarmonen.DataAccess.Entities;

public class WineType
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
}