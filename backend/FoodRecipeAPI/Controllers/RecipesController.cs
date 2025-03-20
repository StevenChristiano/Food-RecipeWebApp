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

        // ✅ Ambil semua daftar resep
        [HttpGet("get-recipes-list")]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _mediator.Send(new GetAllRecipesQuery());
            if (recipes == null || !recipes.Any())
                return NoContent();

            return Ok(recipes);
        }

        // ✅ Ambil satu resep berdasarkan ID
        [HttpGet("get-recipe/{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .Where(r => r.Id == id)
                .Select(r => new UpdateRecipeDto
                {
                    Id = r.Id,
                    RecipeName = r.Name,
                    Details = r.Details,
                    CategoryId = r.CategoryId,
                    Ingredients = r.Ingredients.Select(i => new IngredientDto
                    {
                        Id = i.Id,
                        Name = i.IngredientName,
                        Quantity = i.Quantity
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (recipe == null)
                return NotFound(new { message = "Resep tidak ditemukan" });

            return Ok(recipe);
        }

        [HttpPost("create-recipe")]
        public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeCommand command)
        {
            var recipe = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }

        [HttpPut("update-recipe/{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] UpdateRecipeCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "ID tidak cocok dengan data yang dikirim." });

            var result = await _mediator.Send(command);
            if (!result) return NotFound(new { message = "Resep tidak ditemukan" });

            return NoContent();
        }

        [HttpDelete("delete-recipe/{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var result = await _mediator.Send(new DeleteRecipeCommand { Id = id });
            if (!result) return NotFound(new { message = "Resep tidak ditemukan" });

            return NoContent();
        }
    }
}