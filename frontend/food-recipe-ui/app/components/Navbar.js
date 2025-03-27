"use client";
import { useState } from "react";
import Link from "next/link";
import { Menu, X } from "lucide-react";

export default function Navbar() {
  const [isOpen, setIsOpen] = useState(false);

  return (
    <nav className="bg-yellow-400 text-gray-900 shadow-lg">
      <div className="max-w-6xl mx-auto px-4">
        <div className="flex justify-between items-center py-3">
          {/* Logo */}
          <Link href="/" className="flex items-center space-x-2">
            <span className="text-3xl">🍳</span>
            <span className="text-2xl font-bold">Let Him Cook</span>
          </Link>

          {/* Desktop Menu */}
          <div className="hidden md:flex space-x-6">
            <Link href="/" className="hover:text-gray-700 font-semibold">Recipes</Link>
            <Link href="/new-recipe" className="hover:text-gray-700 font-semibold">Create Recipe</Link>
            <Link href="/update-recipe" className="hover:text-gray-700 font-semibold">Update Recipe</Link>
          </div>

          {/* Mobile Menu Button */}
          <button
            className="md:hidden text-gray-900"
            onClick={() => setIsOpen(!isOpen)}
          >
            {isOpen ? <X size={28} /> : <Menu size={28} />}
          </button>
        </div>

        {/* Mobile Menu */}
        {isOpen && (
          <div className="md:hidden flex flex-col items-center pb-4 space-y-3">
            <Link href="/" className="hover:text-gray-700 font-semibold" onClick={() => setIsOpen(false)}>Recipes</Link>
            <Link href="/new-recipe" className="hover:text-gray-700 font-semibold" onClick={() => setIsOpen(false)}>Create Recipe</Link>
            <Link href="/update-recipe" className="hover:text-gray-700 font-semibold" onClick={() => setIsOpen(false)}>Update Recipe</Link>
          </div>
        )}
      </div>
    </nav>
  );
}