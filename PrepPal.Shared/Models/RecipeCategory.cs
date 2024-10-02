using System.ComponentModel.DataAnnotations;

namespace PrepPal.Models;

public class RecipeCategory
{
    [Key]
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Recipe Recipe { get; set; }
}