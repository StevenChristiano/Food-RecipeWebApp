using System.ComponentModel;
using System.Text.Json.Serialization;

namespace FoodRecipeAPI.Models.Dto
{
    public class IngredientsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
