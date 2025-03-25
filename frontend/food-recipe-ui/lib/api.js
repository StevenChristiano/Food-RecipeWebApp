export async function getAllRecipes() {
  try {
    console.log("🔍 Fetching recipes...");

    const res = await fetch("http://localhost:5267/api/v1/get-recipes-list");

    console.log("📡 Request sent. Response status:", res.status);

    if (!res.ok) {
      throw new Error(`❌ HTTP error! Status: ${res.status}`);
    }

    const data = await res.json();
    console.log("✅ Fetched data:", data);
    return data;
  } catch (error) {
    console.error("🚨 Error fetching recipes:", error.message);
    throw error;
  }
}

export async function getRecipeById(id) {
  console.log("📡 Fetching recipe for ID:", id); // Debugging

  if (!id) {
    console.error("🚨 Error: ID is undefined or null");
    throw new Error("Invalid ID provided");
  }
  
  try {
    const res = await fetch(`http://localhost:5267/api/v1/get-recipe-list/${id}`);
    console.log("📡 Request sent. Response status:", res.status);

    if (!res.ok) {
      throw new Error(`❌ HTTP error! Status: ${res.status}`);
    }

    const data = await res.json();
    console.log("✅ Fetched data:", data);
    return data;
  } catch (error) {
    console.error("🚨 Error fetching recipes:", error.message);
    throw error;
  }
}