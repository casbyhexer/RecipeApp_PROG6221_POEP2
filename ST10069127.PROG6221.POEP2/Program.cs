using System;
using System.Collections.Generic;

public class Recipe
{
    public string Name { get; set; }
    public List<Ingredient> Ingredients { get; set; }

    public delegate void RecipeCaloriesExceededHandler(Recipe recipe);
    public event RecipeCaloriesExceededHandler CaloriesExceeded;

    public Recipe(string name)
    {
        Name = name;
        Ingredients = new List<Ingredient>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        Ingredients.Add(ingredient);
    }

    public int CalculateTotalCalories()
    {
        int totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
    }
}

public class Ingredient
{
    public string Name { get; set; }
    public int Calories { get; set; }
    public string FoodGroup { get; set; }

    public Ingredient(string name, int calories, string foodGroup)
    {
        Name = name;
        Calories = calories;
        FoodGroup = foodGroup;
    }
}

public class Program
{
    private static List<Recipe> recipes = new List<Recipe>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. Display Recipes");
            Console.WriteLine("3. Select Recipe");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Enter your choice:");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddRecipe();
                    break;
                case "2":
                    DisplayRecipes();
                    break;
                case "3":
                    SelectRecipe();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Wrong choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddRecipe()
    {
        Console.WriteLine("Insert recipe name:");
        string name = Console.ReadLine();

        Recipe recipe = new Recipe(name);

        recipes.Add(recipe);
        recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));

        Console.WriteLine("Insert the ingredient details (or type 'done' to finish):");
        while (true)
        {
            Console.WriteLine("Ingredient Name:");
            string ingredientName = Console.ReadLine();
            if (ingredientName.ToLower() == "done")
                break;

            Console.WriteLine("Calories:");
            int calories = int.Parse(Console.ReadLine());

            Console.WriteLine("Food Group:");
            string foodGroup = Console.ReadLine();

            Ingredient ingredient = new Ingredient(ingredientName, calories, foodGroup);
            recipe.AddIngredient(ingredient);
        }
    }

    private static void DisplayRecipes()
    {
        Console.WriteLine("Recipes:");
        foreach (var recipe in recipes)
        {
            Console.WriteLine(recipe.Name);
        }
    }

    private static void SelectRecipe()
    {
        Console.WriteLine("Enter the recipe name:");
        string recipeName = Console.ReadLine();

        Recipe selectedRecipe = recipes.Find(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));

        if (selectedRecipe != null)
        {
            int totalCalories = selectedRecipe.CalculateTotalCalories();
            Console.WriteLine("Total Calories: " + totalCalories);

            if (totalCalories > 300)
            {
                Console.WriteLine("Warning: Calories exceed 300!");
                selectedRecipe.CaloriesExceeded?.Invoke(selectedRecipe);
            }
        }
        else
        {
            Console.WriteLine("Recipe not found.");
        }
    }
}
