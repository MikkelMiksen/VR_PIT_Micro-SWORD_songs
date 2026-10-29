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
}
