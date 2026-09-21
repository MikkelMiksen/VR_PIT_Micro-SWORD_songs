using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour, Iinteractable
{
    [Header("Campfire Recipes")]
    public List<RecipeSO> campfireRecipes = new();

    public string GetPrompt() => "Press E to use Campfire";

    public void Interact()
    {
        // CraftingUIController.Instance.ShowUI(this, campfireRecipes);
    }
}