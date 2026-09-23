using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// RecipeSO is a scriptable object that defines a crafting recipe.
/// </summary>
[CreateAssetMenu(menuName = "Scripts/Crafting")]
public class RecipeSO : ScriptableObject
{
    public string recipeName;
    public ResourceType outputItem;
    public ItemCategory outputCategory = ItemCategory.Material;
    public int outputAmount = 1;

    public GameObject outputPrefab;
    public int toolDurability;

    /// <summary>
    /// Ingredient is a struct that defines an ingredient in a crafting recipe.
    /// </summary>
    [System.Serializable]
    public struct Ingredient
    {
        public ResourceType type;
        public int amount;
    }

    /// <summary>
    /// List of ingredients in the recipe.
    /// </summary>
    public List<Ingredient> ingredients = new();

    /// <summary>
    /// Required station for crafting this recipe.
    /// </summary>
    public WorkstationType requiredStation = WorkstationType.Workbench;
}