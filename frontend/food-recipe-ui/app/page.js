"use client";
import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";

export default function HomePage() {
  const router = useRouter();
  const [recipes, setRecipes] = useState([]);
  const [filteredRecipes, setFilteredRecipes] = useState([]);
  const [search, setSearch] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("");
  const [sortOrder, setSortOrder] = useState("asc");
  const [currentPage, setCurrentPage] = useState(1);
  const recipesPerPage = 6;

  useEffect(() => {
    async function fetchRecipes() {
      try {
        const response = await fetch("http://localhost:5267/api/v1/get-recipes-list");
        if (!response.ok) throw new Error("Failed to fetch recipes");
        const data = await response.json();
        console.log(data.data);
        setRecipes(data.data || []);
      } catch (error) {
        console.error("Failed to fetch recipes:", error);
      }
    }
    fetchRecipes();
  }, []);

  const categories = [...new Set(recipes.map((recipe) => recipe.categoryName))];

  useEffect(() => {
    let filtered = recipes.filter((recipe) =>
      recipe.name.toLowerCase().includes(search.toLowerCase())
    );

    if (selectedCategory) {
      filtered = filtered.filter((recipe) => recipe.categoryName === selectedCategory);
    }

    filtered = filtered.sort((a, b) => {
      if (sortOrder === "asc") return a.name.localeCompare(b.name);
      return b.name.localeCompare(a.name);
    });

    setFilteredRecipes(filtered);
  }, [search, selectedCategory, sortOrder, recipes]);

  const totalPages = Math.ceil(filteredRecipes.length / recipesPerPage);
  const displayedRecipes = filteredRecipes.slice(
    (currentPage - 1) * recipesPerPage,
    currentPage * recipesPerPage
  );

  return (
    <div className="p-6 min-h-screen">
      <h1 className="text-4xl font-bold text-center text-brown-800 mb-6">
        My Recipes
      </h1>

      {/* Filter & Sorting Controls */}
      <div className="flex flex-col md:flex-row justify-center items-center gap-4 bg-[#F4E1C6] p-4 rounded-lg shadow-lg">
        <input
          type="text"
          placeholder="🔍 Search Recipes..."
          className="border border-brown-600 p-3 rounded-lg w-full md:w-1/3 text-brown-800 bg-white"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />

        <select
          className="border border-brown-600 p-3 rounded-lg w-full md:w-1/3 text-brown-800"
          value={selectedCategory}
          onChange={(e) => setSelectedCategory(e.target.value)}
        >
          <option value="">📂 All Categories</option>
          {categories.map((category) => (
            <option key={category} value={category}>
              {category}
            </option>
          ))}
        </select>

        <select
          className="border border-brown-600 p-3 rounded-lg w-full md:w-1/3 text-brown-800"
          value={sortOrder}
          onChange={(e) => setSortOrder(e.target.value)}
        >
          <option value="asc">⬆️ Sort A-Z</option>
          <option value="desc">⬇️ Sort Z-A</option>
        </select>
      </div>

      {/* Recipe List */}
      {displayedRecipes.length === 0 ? (
        <p className="text-brown-700 text-center mt-6">🚫 No Recipes Found...</p>
      ) : (
        <div className="my-12 grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {displayedRecipes.map((recipe, index) => (
            <div
              key={recipe.id || index}
              className="border border-brown-700 bg-[#FFF5E0] rounded-lg p-6 shadow hover:shadow-lg transition-all duration-200"
            >
              <h2 className="font-bold text-xl text-brown-900">{recipe.name}</h2>
              <span className="bg-[#E3B16F] text-white px-3 py-1 rounded-full text-sm inline-block my-2">
                {recipe.categoryName}
              </span>
              <p className="text-brown-700">{recipe.details?.substring(0, 100)}...</p>
              <Link href={`/recipe/${recipe.id}`} className="text-blue-500">
                View Details
              </Link>
            </div>
          ))}
        </div>
      )}

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="flex justify-center mt-6 gap-2">
          <button
            className="rounded-lg px-4 py-2 bg-[#D2691E] text-white shadow hover:bg-[#A0522D] transition"
            disabled={currentPage === 1}
            onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
          >
            Previous
          </button>
          <span className="px-4 py-2 bg-[#EED2B4] text-brown-800 rounded-lg">
            {currentPage} of {totalPages}
          </span>
          <button
            className="rounded-lg px-4 py-2 bg-[#D2691E] text-white shadow hover:bg-[#A0522D] transition"
            disabled={currentPage === totalPages}
            onClick={() => setCurrentPage((prev) => Math.min(prev + 1, totalPages))}
          >
            Next
          </button>
        </div>
      )}
    </div>
  );
}
