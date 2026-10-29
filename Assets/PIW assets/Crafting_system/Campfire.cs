using System.Collections.Generic;
using PIW_assets.crafting_system;
using PIW_asstes.Crafting_system;
using UnityEngine;

namespace PIW_assets.Crafting_system
{
    /// <summary>
    /// Campfire implements CraftingStation and needs pre-defined recipes <typeparam name="RecipeSO"></typeparam> and uses WorkstationType.Campfire.
    /// intended for cooking a slight heat provision. (NOT IMPLEMENTED)
    /// </summary>
    public class Campfire : CraftingStation
    {
        [Header("Campfire Recipes")] public List<RecipeSO> campfireRecipes = new();

        public override WorkstationType StationType => WorkstationType.Campfire;
        public override string StationName => "Campfire";

        /// <summary>
        /// Displays Campfire crafting UI
        /// </summary>
        public override void Interact()
        {
            if (CraftingUIController.Instance != null)
            {
                CraftingUIController.Instance.ShowUI(this, campfireRecipes);
            }
        }

        ///TODO: Implement heat provision and cooking
    }
}