using System.Collections.Generic;
using PIW_assets.crafting_system;
using PIW_assets.Crafting_system;
using PIW_asstes.Crafting_system;
using UnityEngine;

namespace PIW_assets.Crafting_system
{
    /// <summary>
    /// Workbench implements CraftingStation and needs pre-defined recipes <typeparam name="RecipeSO"></typeparam> and uses WorkstationType.Workbench.
    /// </summary>
    public class Workbench : CraftingStation
    {
        [Header("Workbench Recipes")] public List<RecipeSO> recipes = new();

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
}