using FluentValidation;
using FoodRecipeAPI.Application.Queries;
using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
namespace FoodRecipeAPI.Application.Validators
{
    public class RecipeQueryValidator: AbstractValidator<RecipeQueryDto>
    {
        private readonly ApplicationDbContext _context;
        public RecipeQueryValidator(ApplicationDbContext context) 
        {
            _context = context;

            RuleFor(x => x.CategoryName)
               .MustAsync(async (category, cancellation) =>
                   string.IsNullOrEmpty(category) || await _context.Categories.AnyAsync(c => c.Name == category, cancellation)
               )
               .WithMessage("Category not found.")
               .WithErrorCode("404"); // Return 404 Not Found

            RuleFor(x => x.OrderBy)
               .Must(orderBy => new[] { "name", "category" }.Contains(orderBy.ToLower()))
               .WithMessage("Invalid orderBy value. Allowed values: 'Name', 'Category'.")
               .WithErrorCode("400"); // Bad Request

            RuleFor(x => x.OrderState)
                .Must(orderState => new[] { "asc", "desc" }.Contains(orderState.ToLower()))
                .WithMessage("Invalid orderState value. Allowed values: 'asc', 'desc'.")
                .WithErrorCode("400");

            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("Page number must be at least 1.")
                .WithErrorCode("400");
        }
    }
}
