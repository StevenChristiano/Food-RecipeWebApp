using FoodRecipeAPI.Application.Commands;
using FoodRecipeAPI.Application.Queries;
using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodRecipeAPI.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RecipesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-recipes-list")]
        public async Task<IActionResult> GetAllRecipes([FromQuery] RecipeQueryDto queryDto)
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery(queryDto));
            return Ok(recipes);
        }

        [HttpGet("get-recipe-list/{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            var recipe = await _mediator.Send(new GetRecipeByIdQuery{Id = id});
            if(recipe == null)
            {
                return NotFound(new { message = "Recipe is not found..." });
            }
            var recipeDto = new RecipeDetailsDto
            {
                Name = recipe.Name,
                Details = recipe.Details,
                CategoryName = recipe.Category.Name,
                Ingredients = recipe.Ingredients.Select(i => new IngredientsDto
                {
                    Name = i.IngredientName,
                    Quantity = i.Quantity,
                }).ToList()
            };

            return Ok(recipeDto);
        }

        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeCommand command)
        {
            var recipe = await _mediator.Send(command);
            return Ok(recipe);
        }
    }
}
