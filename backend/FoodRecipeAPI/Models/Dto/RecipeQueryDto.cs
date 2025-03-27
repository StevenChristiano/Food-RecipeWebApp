namespace FoodRecipeAPI.Models.Dto
{
    public class RecipeQueryDto // buat tampilan kalo mau setting filter dan sorting
    {
        public string? CategoryName { get; set; }
        public string OrderBy { get; set; } = "name";
        public string OrderState { get; set; } = "asc";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
