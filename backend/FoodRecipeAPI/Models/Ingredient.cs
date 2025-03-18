namespace FoodRecipeAPI.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
