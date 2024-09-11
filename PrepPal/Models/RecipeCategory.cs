namespace PrepPal.Models;

public class RecipeCategory
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public Recipe Recipe { get; set; }
}