using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipeAPI.Services
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;
        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Recipe?> GetRecipeByIdAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}
