using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Crafting_system
{
    public class Campfire : CraftingStation
    {
        [Header("Campfire Recipes")] public List<RecipeSO> campfireRecipes = new();

        public override WorkstationType StationType => WorkstationType.Campfire;
        public override string StationName => "Campfire";

        public override void Interact()
        {
            if (CraftingUIController.Instance != null)
            {
                CraftingUIController.Instance.ShowUI(this, campfireRecipes);
            }
        }
    }
}
