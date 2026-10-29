using UnityEngine;

namespace PIW_assets.Resources
{
    /// <summary>
    /// Resource type enum basis for the whole resource system
    /// </summary>
    public enum ResourceType
    {
        Wood,
        Stone,
        Metal,
        Coal,
        AnimalFat,
        Water,
        Diesel,
        RawMeat,
        CrudeOil,
        CookedMeat,
        Berries,
        Apple,
        StoneAxe,
        StonePickaxe,
        Sword,
        Elucidator,
        DarkRepulser,
        LambentLight,
    }

    /// <summary>
    /// Resource stack, when you stack resources, you get a stack of them
    /// </summary>
    [System.Serializable]
    public class ResourceStack
    {
        public ResourceType type;
        public int amount;

        public ResourceStack(ResourceType type, int amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
