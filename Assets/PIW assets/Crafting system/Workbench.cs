using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour, Iinteractable
{
    [Header("Workbench Recipes")]
    public List<RecipeSO> recipes = new();

    public string GetPrompt() => "Press E to use Workbench";

    public void Interact()
    {
        // Open UI and pass this station + recipes
        // CraftingUIController.Instance.ShowUI(this, recipes);
    }
}