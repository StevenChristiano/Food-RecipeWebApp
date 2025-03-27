"use client";

import { useState, useEffect } from "react";
import { useRouter } from "next/navigation";

export default function UpdateRecipePage() {
  const [recipeId, setRecipeId] = useState("");
  const [recipe, setRecipe] = useState({
    name: "",
    detail: "",
    categoryName: "",
    ingredients: [],
  });
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState("");
  const [loading, setLoading] = useState(false);
  const [modal, setModal] = useState({ isOpen: false, message: "", type: "", onConfirm: null });

  const router = useRouter();

  useEffect(() => {
    setCategories([
      "Appetizer", "Main Course", "Dessert", "Beverages",
      "Vegetarian", "Vegan", "Gluten-Free", "Low Carb",
      "High Protein", "Seafood", "Pasta & Noodles", "Breakfast",
      "Fast Food", "Indonesian Cuisine", "Asian Cuisine",
      "Western Cuisine", "Spicy Food", "BBQ & Grill",
      "Soup & Stew", "Healthy Food"
    ]);
  }, []);

  const fetchRecipe = async () => {
    if (!recipeId) {
      openModal("Please enter a Recipe ID!", "info");
      return;
    }

    try {
      const response = await fetch(`http://localhost:5267/api/v1/get-recipe-list/${recipeId}`);
      if (!response.ok) throw new Error("Recipe not found!");
      const data = await response.json();

      setRecipe({
        name: data.name,
        detail: data.details || "",
        categoryName: data.categoryName,
        ingredients: data.ingredients || [],
      });

      setSelectedCategory(data.categoryName);
    } catch (error) {
      openModal(error.message, "error");
    }
  };

  const openModal = (message, type = "info", onConfirm = null) => {
    setModal({ isOpen: true, message, type, onConfirm });
  };

  const closeModal = () => {
    setModal({ isOpen: false, message: "", type: "", onConfirm: null });
    if (modal.type === "success") {
      setRecipeId("");
      setRecipe({ name: "", detail: "", categoryName: "", ingredients: [] });
      setSelectedCategory("");
      router.push("/update-recipe");
    }
  };

  const removeIngredient = (index) => {
    const newIngredients = recipe.ingredients.filter((_, i) => i !== index);
    setRecipe({ ...recipe, ingredients: newIngredients });
  };

  const addIngredient = () => {
    setRecipe({
      ...recipe,
      ingredients: [...recipe.ingredients, { name: "", quantity: "" }],
    });
  };

  const updateRecipe = async () => {
    if (!recipe.name || !recipe.detail || !selectedCategory) {
      openModal("Name, details, and category are required!", "error");
      return;
    }

    if (recipe.ingredients.some((ing) => !ing.name || !ing.quantity)) {
      openModal("All ingredients must have a name and quantity!", "error");
      return;
    }

    try {
      setLoading(true);
      const response = await fetch(`http://localhost:5267/api/v1/update-recipe/${recipeId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          id: parseInt(recipeId),
          name: recipe.name,
          detail: recipe.detail,
          categoryName: selectedCategory,
          ingredients: recipe.ingredients,
        }),
      });

      if (response.ok) {
        openModal("Recipe updated successfully!", "success");
      } else {
        const errorData = await response.json();
        throw new Error(errorData.message || "Failed to update recipe");
      }
    } catch (error) {
      openModal(error.message, "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-2xl mx-auto p-6">
      <div className="border border-brown-700 bg-[#FFF5E0] rounded-lg p-6 shadow hover:shadow-lg transition-all duration-200">
        <h1 className="text-2xl font-bold text-gray-800 mb-4">Update Recipe</h1>

        <div className="mb-4 flex gap-2">
          <input
            type="text"
            className="border p-2 rounded w-48"
            placeholder="Enter Recipe ID"
            value={recipeId}
            onChange={(e) => setRecipeId(e.target.value)}
          />
          <button className="bg-blue-500 text-white px-4 py-2 rounded" onClick={fetchRecipe}>
            Search Recipe
          </button>
        </div>

        {recipe.name && (
          <form onSubmit={(e) => { e.preventDefault(); updateRecipe(); }}>
            <label className="block font-medium text-gray-700">Recipe Name</label>
            <input
              type="text"
              className="w-full border p-2 rounded-md mb-4"
              value={recipe.name}
              onChange={(e) => setRecipe({ ...recipe, name: e.target.value })}
            />

            <label className="block font-medium text-gray-700">Category</label>
            <select
              value={selectedCategory}
              onChange={(e) => setSelectedCategory(e.target.value)}
              className="w-full border p-2 rounded-md mb-4"
            >
              <option value="" disabled>Select a category</option>
              {categories.map((category) => (
                <option key={category} value={category}>{category}</option>
              ))}
            </select>

            <label className="block font-medium text-gray-700">Details</label>
            <textarea
              className="w-full border p-2 rounded-md mb-4"
              value={recipe.detail}
              onChange={(e) => setRecipe({ ...recipe, detail: e.target.value })}
            />

            <h2 className="text-xl font-semibold mt-4">Ingredients</h2>
            {recipe.ingredients.map((ingredient, index) => (
              <div key={index} className="flex items-center gap-2 mb-3">
                <input
                  type="text"
                  placeholder="Ingredient Name"
                  value={ingredient.name}
                  onChange={(e) => {
                    const newIngredients = [...recipe.ingredients];
                    newIngredients[index].name = e.target.value;
                    setRecipe({ ...recipe, ingredients: newIngredients });
                  }}
                  className="w-1/2 border p-2 rounded-md"
                  required
                />
                <input
                  type="text"
                  placeholder="Quantity"
                  value={ingredient.quantity}
                  onChange={(e) => {
                    const newIngredients = [...recipe.ingredients];
                    newIngredients[index].quantity = e.target.value;
                    setRecipe({ ...recipe, ingredients: newIngredients });
                  }}
                  className="w-1/3 border p-2 rounded-md"
                  required
                />
                <button
                  type="button"
                  className="bg-red-500 text-white px-3 py-1 rounded-md"
                  onClick={() => removeIngredient(index)}
                >
                  ✕
                </button>
              </div>
            ))}

            <button type="button" onClick={addIngredient} className="text-blue-500 hover:underline mb-4">
              + Add Ingredient
            </button>

            <button
              type="submit"
              className="w-full bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-md"
              disabled={loading}
            >
              {loading ? "Updating..." : "Update Recipe"}
            </button>
          </form>
        )}
      </div>
    </div>
  );
}