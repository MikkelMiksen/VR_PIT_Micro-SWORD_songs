using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Workbench implements CraftingStation and needs pre-defined recipes <typeparam name="RecipeSO"></typeparam> and uses WorkstationType.Workbench.
/// </summary>
public class Workbench : CraftingStation
{
    [Header("Workbench Recipes")]
    public List<RecipeSO> recipes = new();

    public override WorkstationType StationType => WorkstationType.Workbench;
    public override string StationName => "Workbench";

    /// <summary>
    /// Displays Workbench crafting UI
    /// </summary>
    public override void Interact()
    {
        if (CraftingUIController.Instance != null)
        {
            CraftingUIController.Instance.ShowUI(this, recipes);
        }
    }
}