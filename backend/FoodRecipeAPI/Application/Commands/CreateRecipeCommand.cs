﻿using FluentValidation;
using FoodRecipeAPI.Data;
using FoodRecipeAPI.Models;
using FoodRecipeAPI.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using ValidationException = FluentValidation.ValidationException;

namespace FoodRecipeAPI.Application.Commands
{
    public class CreateRecipeCommand : IRequest<RecipeDetailsDto>
    {
        public string RecipeName { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public List<IngredientsDto> Ingredients { get; set; } = new();
    }

    public class CreateRecipeHandler : IRequestHandler<CreateRecipeCommand, RecipeDetailsDto>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateRecipeCommand> _validator;
        public CreateRecipeHandler(ApplicationDbContext context, IValidator<CreateRecipeCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<RecipeDetailsDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(
                    error => error.PropertyName,
                    error => new[] { error.ErrorMessage }
                );
                var problemDetails = new ValidationProblemDetails(errors)
                {
                    Type = "https://tools.ietf.org/html/rfc7807",
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest
                };

                throw new ValidationException(problemDetails.Errors.Select(e => new FluentValidation.Results.ValidationFailure(e.Key, e.Value.First())));
            }

                // 🔹 Cari kategori atau buat baru
                var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);

            if (category == null)
            {
                throw new ValidationException("Category does not exist. Please choose from the available categories.");
            }

            // 🔹 Buat Recipe (TANPA Ingredients terlebih dahulu)
            var recipe = new Recipe
            {
                Name = request.RecipeName,
                Details = request.Details,
                CategoryId = category.Id
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync(cancellationToken); // Simpan dulu agar ada ID

            // 🔹 Tambahkan Ingredients (Gunakan RecipeId)
            var ingredients = request.Ingredients.Select(i => new Ingredient
            {
                RecipeId = recipe.Id,  // Hubungkan ke Recipe yang baru dibuat
                IngredientName = i.Name,
                Quantity = i.Quantity
            }).ToList();

            _context.Ingredients.AddRange(ingredients);
            await _context.SaveChangesAsync(cancellationToken);

            // 🔹 Fetch ulang Recipe dari database dengan `Include`
            var savedRecipe = await _context.Recipes
                .Include(r => r.Ingredients) // Pastikan Ingredients di-load
                .FirstOrDefaultAsync(r => r.Id == recipe.Id, cancellationToken);

            if (savedRecipe == null)
            {
                throw new Exception("Failed to retrieve saved recipe");
            }

            // 🔹 Konversi ke DTO
            var recipeDto = new RecipeDetailsDto
            {
                Name = savedRecipe.Name,
                Details = savedRecipe.Details,
                CategoryName = category.Name,
                Ingredients = savedRecipe.Ingredients.Select(i => new IngredientsDto
                {
                    Name = i.IngredientName,
                    Quantity = i.Quantity
                }).ToList()
            };

            return recipeDto;
        }

    }

}
