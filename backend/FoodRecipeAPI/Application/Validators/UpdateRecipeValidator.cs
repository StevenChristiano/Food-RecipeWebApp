using FluentValidation;
using FoodRecipeAPI.Application.Commands;

namespace FoodRecipeAPI.Application.Validators
{
    public class UpdateRecipeValidator : AbstractValidator<UpdateRecipeCommand>
    {
        public UpdateRecipeValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Recipe Id must be greater than 0.");

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Recipe name cannot exceed 100 characters.");

            RuleFor(x => x.CategoryName)
                .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters.");

            RuleFor(x => x.Ingredients)
                .Must(i => i == null || i.All(ingredient => !string.IsNullOrEmpty(ingredient.Name) && !string.IsNullOrEmpty(ingredient.Quantity)))
                .WithMessage("Each ingredient must have a name and quantity.");
        }
    }
}