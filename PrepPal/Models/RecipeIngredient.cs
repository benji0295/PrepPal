namespace PrepPal.Models;

public class RecipeIngredient
{
    public int RecipeIngredientId { get; set; }
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public decimal Quantity { get; set; }
    public string Unit { get; set; }

    public Recipe Recipe { get; set; }
    public Ingredient Ingredient { get; set; }
}