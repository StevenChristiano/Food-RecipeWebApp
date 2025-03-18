namespace FoodRecipeAPI.Models.Dto
{
    public class RecipeDetailsDto
    {
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string Details { get; set; }
        public List<IngredientsDto> Ingredients { get; set; }
    }
}
