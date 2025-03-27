using FluentValidation;
using FoodRecipeAPI.Application.Queries;

namespace FoodRecipeAPI.Application.Validators
{
    public class GetRecipeByIdQueryValidator:AbstractValidator<GetRecipeByIdQuery>  
    {
        public GetRecipeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Recipe Id must be greater than 0.")
                .WithErrorCode("400");
        }
    }
}
