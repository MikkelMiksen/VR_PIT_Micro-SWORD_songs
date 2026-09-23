using UnityEngine;

/// <summary>
/// Resource type enum basis for the whole resource system
/// </summary>
public enum ResourceType
{
    //Materials
    Wood, // Sprite is added to GetSpriteForType()
    Stone, // Sprite is added to GetSpriteForType()
    Metal, // Sprite is added to GetSpriteForType()
    Coal, // Sprite is added to GetSpriteForType()
    Water, // Sprite is added to GetSpriteForType()
    Diesel, // Sprite is added to GetSpriteForType()
    RawMeat, // Sprite is added to GetSpriteForType()
    CrudeOil, // Missing sprite
    //Consumables
    CookedMeat, // Sprite is added to GetSpriteForType()
    Berries, // Sprite is added to GetSpriteForType()
    //Tools
    StoneAxe, // Sprite is added to GetSpriteForType()
    StonePickaxe, // Sprite is added to GetSpriteForType()
    Sword, // Added for Phase 3
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