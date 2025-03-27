using System.Collections.Generic;

namespace FoodRecipeAPI.Models.Dto
{
    public class UpdateRecipeDto
    {
        public int Id { get; set; }
        public string? RecipeName { get; set; }  // Bisa null
        public string? Details { get; set; }  // Bisa null
        public int? CategoryId { get; set; }  // Bisa null

        public List<IngredientsDto>? Ingredients { get; set; }  // Bisa null
    }
}