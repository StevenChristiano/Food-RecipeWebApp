using System.Collections.Generic;

namespace FoodRecipeAPI.Models.Dto
{
    public class UpdateRecipeDto
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
    }

    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}