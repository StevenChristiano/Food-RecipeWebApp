"use client";
import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";

export default function RecipeDetail() {
  const { id } = useParams();
  const router = useRouter();
  const [recipe, setRecipe] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    async function fetchRecipe() {
      try {
        const res = await fetch(`http://localhost:5267/api/v1/get-recipe-list/${id}`);
        if (!res.ok) throw new Error("Failed to fetch recipe");
        const data = await res.json();
        setRecipe(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchRecipe();
  }, [id]);

  if (loading) return <p className="text-center text-gray-500">Loading...</p>;
  if (error) return <p className="text-center text-red-500">Error: {error}</p>;
  if (!recipe) return <p className="text-center text-gray-500">Recipe not found</p>;

  return (
    <div className="max-w-3xl mx-auto p-6">
      {/* Card Layout */}
      <div className="border border-brown-700 bg-[#FFF5E0] rounded-lg p-6 shadow hover:shadow-lg transition-all duration-200">
        {/* Recipe Image */}
        {/* <img 
          src={recipe.imageUrl || "/placeholder.jpg"} 
          alt={recipe.name} 
          className="w-full h-60 object-cover rounded-md mb-4"
        /> */}

        {/* Recipe Details */}
        <h1 className="text-3xl font-bold text-gray-800">{recipe.name}</h1>
        <p className="text-gray-600 mt-2">{recipe.details}</p>

        {/* Ingredients Section */}
        <h2 className="mt-6 text-xl font-semibold text-gray-800">Ingredients</h2>
        <ul className="list-disc ml-5 mt-2 text-gray-700">
          {(recipe.ingredients ?? []).map((item, index) => (
            <li key={index} className="py-1">
              <span className="font-medium">{item.name}</span> - {item.quantity}
            </li>
          ))}
        </ul>

        {/* Back Button */}
        <button 
          onClick={() => router.back()} 
          className="mt-6 bg-[#D2691E] hover:bg-[#A0522D] transition text-white px-4 py-2 rounded-md duration-200"
        >
          Back to Recipes
        </button>
      </div>
    </div>
  );
}
