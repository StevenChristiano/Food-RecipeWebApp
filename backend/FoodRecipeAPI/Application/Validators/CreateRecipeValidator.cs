using FluentValidation;
using FoodRecipeAPI.Application.Commands;
using FoodRecipeAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipeAPI.Application.Validators
{
    public class CreateRecipeValidator: AbstractValidator<CreateRecipeCommand>
    {
        private readonly ApplicationDbContext _context;

        public CreateRecipeValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.RecipeName)
               .NotEmpty().WithMessage("Recipe name is required.")
               .MaximumLength(100).WithMessage("Recipe name must not exceed 100 characters.")
               .WithErrorCode("400");

            RuleFor(x => x.Details)
                .NotEmpty()
                .WithMessage("Recipe details are required.")
                .WithErrorCode("400");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category is required.")
                .WithErrorCode("400");

            RuleFor(x => x.CategoryName)
               .MustAsync(CategoryExists)
               .WithMessage("Category does not exist. Please choose from the available categories.")
               .WithErrorCode("404");
        }
        private async Task<bool> CategoryExists(string categoryName, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.Name == categoryName, cancellationToken);
        }
    }
}
