using FoodRecipeAPI.Application.Commands;
using FoodRecipeAPI.Application.Queries;
using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodRecipeAPI.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class RecipesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ApplicationDbContext _context;

        public RecipesController(IMediator mediator, ApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
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
            var recipe = await _mediator.Send(new GetRecipeByIdQuery { Id = id });
            if (recipe == null)
            {
                return NotFound(new { message = "Recipe is not found..." });
            }
            var recipeDto = new RecipeDetailsDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Details = recipe.Details,
                CategoryName = recipe.Category.Name,
                Ingredients = recipe.Ingredients.Select(i => new IngredientsDto
                {
                    Id = i.Id,
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

        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new { c.Id, c.Name })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpPut("update-recipe/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] UpdateRecipeCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Bad Request",
                    Status = 400,
                    Detail = "ID does not match with request body.",
                    Instance = HttpContext.Request.Path
                });
            }

            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Status = 404,
                    Detail = "Recipe not found or invalid category ID.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }

        [HttpDelete("delete-recipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _mediator.Send(new DeleteRecipeCommand { Id = id });
            if (!result)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Not Found",
                    Status = 404,
                    Detail = "Recipe not found or invalid category ID.",
                    Instance = HttpContext.Request.Path
                });
            }

            return NoContent();
        }
    }
}