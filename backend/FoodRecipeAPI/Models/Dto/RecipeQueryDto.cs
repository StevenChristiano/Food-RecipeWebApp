namespace FoodRecipeAPI.Models.Dto
{
    public class RecipeQueryDto // buat tampilan kalo mau setting filter dan sorting
    {
        public string? CategoryName { get; set; } // Filter by category
        public string OrderBy { get; set; } = "name"; // Sorting (default by name)
        public string OrderState { get; set; } = "asc"; // Sorting order (asc/desc)
        public int Page { get; set; } = 1; // Page number
        public int PageSize { get; set; } = 10; // Items per page
    }
}
