using System.Collections.Generic;
using UnityEngine;

public class Refinery : MonoBehaviour, Iinteractable
{
    public List<RecipeSO> refineryRecipes = new();

    public string GetPrompt() => "Press E to use Refinery";

    public void Interact()
    {
        // CraftingUIController.Instance.ShowUI(this, refineryRecipes);
    }
}