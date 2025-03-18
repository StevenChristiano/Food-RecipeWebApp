using FluentValidation;
namespace FoodRecipeAPI.Application.Validators
{
    public class RecipeQueryValidator: AbstractValidator<int>
    {
        public RecipeQueryValidator() 
        {
            RuleFor(count => count)
                .GreaterThan(0)
                .WithMessage("Belum Memiliki Resep");
        }
    }
}
