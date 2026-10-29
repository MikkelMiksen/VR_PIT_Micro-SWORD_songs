using System.Collections.Generic;
using UnityEngine;

public class MJ_PlayerInventory : MonoBehaviour
{
    public static MJ_PlayerInventory Instance;

    [SerializeField] private List<InventorySlot> resources = new();
    [SerializeField] private List<Image> images = new();
    [SerializeField] private List<TextMeshProUGUI> inventoryNumberDisplay = new List<TextMeshProUGUI>();
    [SerializeField] GameObject inventoryGlobalLayout;

    [SerializeField] private Sprite woodSprite,
        stoneSprite, metalSprite,
        coalSprite, waterSprite,
        dieselSprite, rawMeatSprite,
        cookedMeatSprite, berriesSprite,
        stoneAxeSprite, stonePickaxeSprite,
        swordSprite,
        crudeOilSprite;

    private int numOfInventorySlots = 88;

    private static readonly HashSet<ResourceType> AutoHudToolTypes = new HashSet<ResourceType>();

    public static bool IsAutoRegisteredTool(ResourceType type)
    {
        return AutoHudToolTypes.Contains(type);
    }

    public static void RegisterToolForHud(ResourceType type)
    {
        if (ResourceData.IsToolResource(type))
        {
            AutoHudToolTypes.Add(type);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    public void AddResource(ResourceType type, int amount, int durability = 0)
    {
        RegisterToolForHud(type);

        var cat = GetCategory(type);

        if (cat == ItemCategory.Tool)
        {
            int toAdd = amount;
            while (toAdd > 0 && resources.Count < numOfInventorySlots)
            {
                resources.Add(new InventorySlot(type, 1, durability));
                toAdd--;
            }
            if (toAdd > 0) Debug.LogWarning("Inventory full, couldn't add all tools");
            AutoSortInventory();
            UpdateResourcesUIText();
            return;
        }

        int remaining = amount;

        foreach (var slot in resources)
        {
            if (slot.type == type && !slot.IsFull)
            {
                int space = 128 - slot.amount;
                int toAdd = Mathf.Min(space, remaining);
                slot.amount += toAdd;
                remaining -= toAdd;
                if (remaining <= 0)
                {
                    AutoSortInventory();
                    UpdateResourcesUIText();
                    return;
                }
            }
        }

        while (remaining > 0 && resources.Count < numOfInventorySlots)
        {
            int toAdd = Mathf.Min(128, remaining);
            resources.Add(new InventorySlot(type, toAdd));
            remaining -= toAdd;
        }

        if (remaining > 0)
            Debug.LogWarning($"❌ Inventory full! Could not add remaining {remaining}x {type}");

        AutoSortInventory();
        UpdateResourcesUIText();
    }

    public ItemCategory GetCategory(ResourceType t)
    {
        switch (t)
        {
            case ResourceType.StoneAxe:
            case ResourceType.StonePickaxe:
            case ResourceType.Sword:
            case ResourceType.Elucidator:
            case ResourceType.DarkRepulser:
            case ResourceType.LambentLight:
                return ItemCategory.Tool;
            case ResourceType.CookedMeat:
            case ResourceType.Berries:
                return ItemCategory.Consumable;
            default:
                return ItemCategory.Material;
        }
    }
}
