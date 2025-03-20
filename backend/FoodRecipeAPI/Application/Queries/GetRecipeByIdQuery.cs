using FoodRecipeAPI.Models;
using FoodRecipeAPI.Services;
using MediatR;

namespace FoodRecipeAPI.Application.Queries
{
    public class GetRecipeByIdQuery : IRequest<Recipe?>
    {
        public int Id { get; set; } 
    }

    public class GetRecipeByIdHandler : IRequestHandler<GetRecipeByIdQuery, Recipe?>
    {
        private readonly RecipeService _recipeService;

        public GetRecipeByIdHandler(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        public async Task<Recipe?> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _recipeService.GetRecipeByIdAsync(request.Id);
        }
    }
}
