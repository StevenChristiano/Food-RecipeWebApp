using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodRecipeAPI.Application.Commands
{
    public class UpdateRecipeCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string Details { get; set; }
        public int CategoryId { get; set; }
        public List<UpdateIngredientCommand> Ingredients { get; set; }
    }

    public class UpdateIngredientCommand
    {
        public int Id { get; set; }  // Bisa 0 jika ingredient baru
        public string Name { get; set; }
        public string Quantity { get; set; }
    }

    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public UpdateRecipeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == request.Id);

            if (recipe == null) return false;

            // Update Resep
            recipe.Name = request.RecipeName;
            recipe.Details = request.Details;
            recipe.CategoryId = request.CategoryId;

            // Update Ingredients
            foreach (var ingredient in request.Ingredients)
            {
                var existingIngredient = recipe.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
                if (existingIngredient != null)
                {
                    // Update existing ingredient
                    existingIngredient.IngredientName = ingredient.Name;
                    existingIngredient.Quantity = ingredient.Quantity;
                }
                else
                {
                    // Tambahkan bahan baru
                    recipe.Ingredients.Add(new Ingredient
                    {
                        IngredientName = ingredient.Name,
                        Quantity = ingredient.Quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}