using FoodRecipeAPI.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodRecipeAPI.Application.Commands
{
    public class DeleteRecipeCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteRecipeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (recipe == null) return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}