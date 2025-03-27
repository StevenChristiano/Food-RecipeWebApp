using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using FoodRecipeAPI.Models.Dto;
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
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? CategoryName { get; set; }
        public List<IngredientsDto>? Ingredients { get; set; }
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
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recipe == null) return false;

            if (!string.IsNullOrEmpty(request.Name))
                recipe.Name = request.Name;

            if (!string.IsNullOrEmpty(request.Details))
                recipe.Details = request.Details;

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);

                if (category != null)
                    recipe.CategoryId = category.Id;
            }

            if (request.Ingredients != null)
            {
                // Hapus ingredient yang tidak ada dalam request
                var ingredientIdsInRequest = request.Ingredients.Select(i => i.Id).ToList();
                recipe.Ingredients.RemoveAll(i => !ingredientIdsInRequest.Contains(i.Id));

                foreach (var ingredient in request.Ingredients)
                {
                    var existingIngredient = recipe.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);

                    if (existingIngredient != null)
                    {
                        existingIngredient.IngredientName = ingredient.Name ?? existingIngredient.IngredientName;
                        existingIngredient.Quantity = ingredient.Quantity ?? existingIngredient.Quantity;
                    }
                    else if (ingredient.Id == 0 && !string.IsNullOrEmpty(ingredient.Name) && !string.IsNullOrEmpty(ingredient.Quantity))
                    {
                        recipe.Ingredients.Add(new Ingredient
                        {
                            IngredientName = ingredient.Name,
                            Quantity = ingredient.Quantity
                        });
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}