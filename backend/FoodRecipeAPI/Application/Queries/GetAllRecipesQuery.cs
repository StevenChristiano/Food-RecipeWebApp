using FluentValidation;
using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using FoodRecipeAPI.Models.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipeAPI.Application.Queries
{
    public class GetAllRecipesQuery: IRequest<PageResponseDto<RecipeListDto>> 
    {
        public RecipeQueryDto Query { get; set; }
        public GetAllRecipesQuery(RecipeQueryDto query)
        {
            Query = query;
        }
    }

    public class GetAllRecipesHandler : IRequestHandler<GetAllRecipesQuery, PageResponseDto<RecipeListDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<RecipeQueryDto> _validator;
        public GetAllRecipesHandler (ApplicationDbContext context, IValidator<RecipeQueryDto> validator)
        {
            _context = context;
            _validator = validator;
        }
        public async Task<PageResponseDto<RecipeListDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            // Validate request
            var validationResult = await _validator.ValidateAsync(request.Query, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            //Filter by CategoryName
            var query = _context.Recipes
                .Where(r => string.IsNullOrEmpty(request.Query.CategoryName) || r.Category.Name == request.Query.CategoryName)
                .Select(r => new RecipeListDto
                {
                    Name = r.Name,
                    CategoryName = r.Category.Name,
                    Details = r.Details
                });

            //if (!string.IsNullOrWhiteSpace(request.Query.CategoryName))
            //{
            //    query = query.Where(r => r.Category.Name == request.CategoryName);
            //}

            //Sorting
            if(request.Query.OrderBy.ToLower() == "name") // Default -> sort by Name
            {
                query = request.Query.OrderState.ToLower() == "asc"
                    ? query.OrderBy(r => r.Name)
                    : query.OrderByDescending(r => r.Name);
            }
            else if (request.Query.OrderBy.ToLower() == "category")
            {
                query = request.Query.OrderState.ToLower() == "asc"
                    ? query.OrderBy(r => r.CategoryName)
                    : query.OrderByDescending(r => r.CategoryName);
            }

            var totalRecipes = await query.CountAsync(cancellationToken);
            //Pagination

            var recipes = await query
                .Skip((request.Query.Page - 1) * request.Query.PageSize)
                .Take(request.Query.PageSize)
                .ToListAsync(cancellationToken);

            return new PageResponseDto<RecipeListDto>
            {
                Data = recipes,
                Page =request.Query.Page,
                PageSize = request.Query.PageSize,
                TotalItems = totalRecipes,
            };        
        }
    }
}
