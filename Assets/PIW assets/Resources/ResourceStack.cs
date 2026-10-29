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
    AnimalFat, //TODO: Add sprite
    Water, // Sprite is added to GetSpriteForType()
    Diesel, // Sprite is added to GetSpriteForType()
    RawMeat, // Sprite is added to GetSpriteForType()
    CrudeOil, //TODO: Add sprite
    //Consumables
    CookedMeat, // Sprite is added to GetSpriteForType()
    Berries, // Sprite is added to GetSpriteForType()
    Apple, //TODO: Add sprite
    //Tools
    StoneAxe, // Sprite is added to GetSpriteForType()
    StonePickaxe, // Sprite is added to GetSpriteForType()
    Sword, //TODO: Add sprite
    Elucidator, //TODO: Add sprite
    DarkRepulser, //TODO: Add sprite
    LambentLight, //TODO: Add sprite
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