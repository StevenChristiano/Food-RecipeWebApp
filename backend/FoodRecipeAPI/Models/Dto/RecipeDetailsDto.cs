namespace FoodRecipeAPI.Models.Dto
{
    public class RecipeDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public List<IngredientsDto> Ingredients { get; set; }
    }
}