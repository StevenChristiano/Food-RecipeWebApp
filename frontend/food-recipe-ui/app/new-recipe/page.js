"use client";
import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";

export default function CreateRecipe() {
  const router = useRouter();
  const [recipe, setRecipe] = useState({
    recipeName: "",
    details: "",
    categoryName: "",
    ingredients: [{ name: "", quantity: "" }],
  });

  const [categories, setCategories] = useState([]); // Store categories
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Fetch categories (Static or API)
  useEffect(() => {
    async function fetchCategories() {
      //manually make it since I don't make getCategories API
      try {
        // Replace with API call if needed
        setCategories([
          "Appetizer",
          "Main Course",
          "Dessert",
          "Beverages",
          "Vegetarian",
          "Vegan",
          "Gluten-Free",
          "Low Carb",
          "High Protein",
          "Seafood",
          "Pasta & Noodles",
          "Breakfast",
          "Fast Food",
          "Indonesian Cuisine",
          "Asian Cuisine",
          "Western Cuisine",
          "Spicy Food",
          "BBQ & Grill",
          "Soup & Stew",
          "Healthy Food"
        ]);
      } catch (err) {
        console.error("Failed to fetch categories:", err);
      }
    }

    fetchCategories();
  }, []);

  // Handle input changes
  const handleChange = (e) => {
    setRecipe({ ...recipe, [e.target.name]: e.target.value });
  };

  // Handle ingredient input change
  const handleIngredientChange = (index, e) => {
    const newIngredients = [...recipe.ingredients];
    newIngredients[index][e.target.name] = e.target.value;
    setRecipe({ ...recipe, ingredients: newIngredients });
  };

  // Add ingredient
  const addIngredient = () => {
    setRecipe({
      ...recipe,
      ingredients: [...recipe.ingredients, { name: "", quantity: "" }],
    });
  };

  // Remove ingredient
  const removeIngredient = (index) => {
    const newIngredients = recipe.ingredients.filter((_, i) => i !== index);
    setRecipe({ ...recipe, ingredients: newIngredients });
  };

  // Handle form submit
  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const res = await fetch("http://localhost:5267/api/v1/create-recipe", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(recipe),
      });

      if (!res.ok) throw new Error("Failed to create recipe");

      router.push("/"); // Redirect to recipes list
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto p-6">
      <div className="border border-brown-700 bg-[#FFF5E0] rounded-lg p-6 shadow hover:shadow-lg transition-all duration-200">
        <h1 className="text-2xl font-bold text-gray-800 mb-4">Create New Recipe</h1>

        {error && <p className="text-red-500 mb-4">{error}</p>}

        <form onSubmit={handleSubmit}>
          {/* Recipe Name */}
          <label className="block font-medium text-gray-700">Recipe Name</label>
          <input
            type="text"
            name="recipeName"
            value={recipe.recipeName}
            onChange={handleChange}
            className="w-full border p-2 rounded-md mb-4"
            required
          />

          {/* Category - Dropdown */}
          <label className="block font-medium text-gray-700">Category</label>
          <select
            name="categoryName"
            value={recipe.categoryName}
            onChange={handleChange}
            className="w-full border p-2 rounded-md mb-4"
            required
          >
            <option value="" disabled>Select a category</option>
            {categories.map((category) => (
              <option key={category} value={category}>
                {category}
              </option>
            ))}
          </select>

          {/* Details */}
          <label className="block font-medium text-gray-700">Details</label>
          <textarea
            name="details"
            value={recipe.details}
            onChange={handleChange}
            className="w-full border p-2 rounded-md mb-4"
            rows="3"
            required
          />

          {/* Ingredients */}
          <h2 className="text-xl font-semibold mt-4">Ingredients</h2>
          {recipe.ingredients.map((ingredient, index) => (
            <div key={index} className="flex items-center gap-2 mb-3">
              <input
                type="text"
                name="name"
                placeholder="Ingredient Name"
                value={ingredient.name}
                onChange={(e) => handleIngredientChange(index, e)}
                className="w-1/2 border p-2 rounded-md"
                required
              />
              <input
                type="text"
                name="quantity"
                placeholder="Quantity"
                value={ingredient.quantity}
                onChange={(e) => handleIngredientChange(index, e)}
                className="w-1/3 border p-2 rounded-md"
                required
              />
              {recipe.ingredients.length > 1 && (
                <button
                  type="button"
                  onClick={() => removeIngredient(index)}
                  className="bg-red-500 text-white px-3 py-1 rounded-md"
                >
                  ✕
                </button>
              )}
            </div>
          ))}

          <button
            type="button"
            onClick={addIngredient}
            className="text-blue-500 hover:underline mb-4"
          >
            + Add Ingredient
          </button>

          {/* Submit Button */}
          <button
            type="submit"
            className="w-full bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-md"
            disabled={loading}
          >
            {loading ? "Submitting..." : "Create Recipe"}
          </button>
        </form>
      </div>
    </div>
  );
}
