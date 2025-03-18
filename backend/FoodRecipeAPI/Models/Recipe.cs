using System.Text.Json.Serialization;

namespace FoodRecipeAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public int CategoryId { get; set; }

        [JsonIgnore]
        public Category Category { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
