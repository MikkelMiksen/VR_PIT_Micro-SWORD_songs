using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Crafting_system
{
    public class Workbench : CraftingStation
    {
        [Header("Workbench Recipes")] public List<RecipeSO> recipes = new();
        public override WorkstationType StationType => WorkstationType.Workbench;
        public override string StationName => "Workbench";

        public override void Interact()
        {
            if (CraftingUIController.Instance != null)
            {
                CraftingUIController.Instance.ShowUI(this, recipes);
            }
        }
    }

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

    public class Refinery : CraftingStation
    {
        public List<RecipeSO> refineryRecipes = new();
        public override WorkstationType StationType => WorkstationType.Refinery;
        public override string StationName => "Refinery";

        public override void Interact()
        {
            if (CraftingUIController.Instance != null)
            {
                CraftingUIController.Instance.ShowUI(this, refineryRecipes);
            }
        }
    }
}
