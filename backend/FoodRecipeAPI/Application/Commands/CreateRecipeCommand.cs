using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FoodRecipeAPI.Application.Commands
{
    public class CreateRecipeCommand: IRequest<Recipe>
    {
        public string RecipeName { get; set; } = string.Empty;
        public string Details { get; set;} = string.Empty;
        public string CategoryName { get; set; } = string.Empty ;
    }

    public class CreateRecipeHandler : IRequestHandler<CreateRecipeCommand, Recipe>
    {
        private readonly ApplicationDbContext _context;
        public CreateRecipeHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Recipe> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            // buat cek apakah di db ada category
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name.ToLower() == request.CategoryName.ToLower(), cancellationToken);

            //kalo hasilnya null, berarti ga ada, maka buat category baru
            if (category == null)
            {
                category = new Category { Name = request.CategoryName };
                _context.Categories.Add(category);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var recipe = new Recipe
            {
                Name = request.RecipeName,
                Details = request.Details,
                CategoryId = category.Id,
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync(cancellationToken);
            return recipe;





        }
    }
}
