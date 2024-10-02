using System.ComponentModel.DataAnnotations;

namespace PrepPal.Models;

public class UnitOfMeasure
{
    [Key]
    public int UnitId { get; set; }
    public string UnitName { get; set; }
    public string UnitAbbreviation { get; set; }
}