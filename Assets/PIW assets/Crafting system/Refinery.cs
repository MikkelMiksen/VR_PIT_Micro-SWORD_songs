using System.Collections.Generic;
using UnityEngine;

public class Refinery : CraftingStation
{
    /// <summary>
    /// Refinery recipes    
    /// </summary>
    public List<RecipeSO> refineryRecipes = new();

    /// <summary>
    /// Refinery station type   
    /// </summary>
    public override WorkstationType StationType => WorkstationType.Refinery;
    public override string StationName => "Refinery";

    /// <summary>
    /// Displays Refinery crafting UI   
    /// </summary>
    public override void Interact()
    {
        if (CraftingUIController.Instance != null)
        {
            CraftingUIController.Instance.ShowUI(this, refineryRecipes);
        }
    }
}