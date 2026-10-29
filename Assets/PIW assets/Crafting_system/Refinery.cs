using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Crafting_system
{
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
