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
        public async Task<ActionResult<List<RecipeListDto>>> GetRecipes()
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            return Ok(recipes);
        }

        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeCommand command)
        {
            var recipe = await _mediator.Send(command);
            return Ok(recipe);
        }
    }
}
