using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using FoodRecipeAPI.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipeAPI.Application.Queries
{
    public class GetAllRecipesQuery: IRequest<List<RecipeListDto>> 
    {
    }

    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipesQuery, List<RecipeListDto>>
    {
        private readonly ApplicationDbContext _context;
        public GetAllRecipesHandler (ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<RecipeListDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _context.Recipes
                .Select(r => new RecipeListDto
                {
                    Name = r.Name,
                    CategoryName = r.Category.Name,
                    Details = r.Details,
                }).ToListAsync(cancellationToken);

            return recipes;        
        }
    }
}
