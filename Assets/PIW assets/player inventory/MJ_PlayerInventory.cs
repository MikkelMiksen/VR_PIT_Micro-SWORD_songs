using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventorySlot
{
    public ResourceType type;
    public int amount;

    public int durability;

    public bool IsEmpty => amount <= 0;
    public bool IsFull => amount >= 128;

    public InventorySlot(ResourceType type, int amount, int durability = 0)
    {
        this.type = type;
        this.amount = amount;
        this.durability = durability;
    }
}

public class MJ_PlayerInventory : MonoBehaviour
{
    public static MJ_PlayerInventory Instance;

    [SerializeField] private List<InventorySlot> resources = new List<InventorySlot>();
    public List<InventorySlot> Resources => resources;


    [SerializeField] private List<Image> images = new List<Image>();

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

    void Awake()
    {
            Instance = this;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(.1f);
        foreach (var theImage in inventoryGlobalLayout.GetComponentsInChildren<Image>())
        {
            images.Add(theImage);
        }
        yield return new WaitForSeconds(.1f);
        foreach (var text in inventoryGlobalLayout.GetComponentsInChildren<TextMeshProUGUI>())
        {
            inventoryNumberDisplay.Add(text);
        }
    }

    public void AddResource(ResourceType type, int amount, int durability = 0)
    {

        var cat = GetCategory(type);

        if (cat == ItemCategory.Tool)
        {
            // Tools don't stack; add 'amount' times as separate slots but each with amount=1
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

        // Fill existing stacks first
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

        // Create new stacks
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

    public List<(ResourceType type, int count)> GetConsumablesList()
    {
        Dictionary<ResourceType, int> map = new Dictionary<ResourceType, int>();
        foreach (var slot in resources)
        {
            if (GetCategory(slot.type) != ItemCategory.Consumable) continue;
            if (!map.ContainsKey(slot.type)) map[slot.type] = 0;
            map[slot.type] += slot.amount;
        }

        var list = new List<(ResourceType, int)>();
        foreach (var kv in map) list.Add((kv.Key, kv.Value));
        return list;
    }

    public List<InventorySlot> GetToolsList()
    {
        var list = new List<InventorySlot>();
        foreach (var s in resources)
            if (GetCategory(s.type) == ItemCategory.Tool)
                list.Add(s);
        return list;
    }

    [Header("Cycling References")]
    [SerializeField] private Transform toolSpawnPoint;
    [SerializeField] private List<ToolMapping> toolMappings = new List<ToolMapping>();
    private GameObject currentToolInstance;
    private int currentToolIndex = -1;
    private int currentFoodIndex = -1;

    [System.Serializable]
    public struct ToolMapping
    {
        public ResourceType type;
        public GameObject prefab;
    }

    public void CycleTools()
    {
        var tools = GetToolsList();
        if (tools.Count == 0) return;

        currentToolIndex = (currentToolIndex + 1) % tools.Count;
        EquipTool(tools[currentToolIndex]);
    }

    public void CycleFood()
    {
        var consumables = GetConsumablesList();
        if (consumables.Count == 0) return;

        currentFoodIndex = (currentFoodIndex + 1) % consumables.Count;
        var food = consumables[currentFoodIndex];
        Debug.Log($"[Inventory] Selected food: {food.type}");
        
        // Phase 4: Handle eating (Eat one if triggered, for now just select)
        // If we want to eat immediately on Y press:
        // EatFood(food.type);
    }

    private void EatFood(ResourceType type)
    {
        if (RemoveResource(type, 1))
        {
            float nutrition = 20f; // Default
            if (type == ResourceType.CookedMeat) nutrition = 40f;
            else if (type == ResourceType.Berries) nutrition = 10f;

            if (MJ_PlayerHunger.Instance != null)
            {
                MJ_PlayerHunger.Instance.Eat(nutrition);
            }
        }
    }

    private void EquipTool(InventorySlot slot)
    {
        if (currentToolInstance != null)
        {
            Destroy(currentToolInstance);
        }

        GameObject prefab = GetPrefabForTool(slot.type);
        if (prefab != null && toolSpawnPoint != null)
        {
            currentToolInstance = Instantiate(prefab, toolSpawnPoint.position, toolSpawnPoint.rotation);
            currentToolInstance.transform.SetParent(toolSpawnPoint);
            
            // If it's a sword, we might need special initialization
            SwordInteraction sword = currentToolInstance.GetComponent<SwordInteraction>();
            if (sword != null)
            {
                // Initialize sword if needed
            }
        }
        
        Debug.Log($"[Inventory] Equipped tool: {slot.type}");
    }

    private GameObject GetPrefabForTool(ResourceType type)
    {
        foreach (var mapping in toolMappings)
        {
            if (mapping.type == type) return mapping.prefab;
        }
        return null;
    }

    public ItemCategory GetCategory(ResourceType t)
    {
        switch (t)
        {
            case ResourceType.StoneAxe:
            case ResourceType.StonePickaxe:
            case ResourceType.Sword:
                return ItemCategory.Tool;
            case ResourceType.CookedMeat:
            case ResourceType.Berries:
                return ItemCategory.Consumable;
            default:
                return ItemCategory.Material;
        }
    }

    private void MergeStacks(ResourceType type)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].type != type || resources[i].IsFull)
                continue;

            for (int j = i + 1; j < resources.Count; j++)
            {
                if (resources[j].type == type && !resources[j].IsEmpty)
                {
                    int space = 128 - resources[i].amount;
                    int transfer = Mathf.Min(space, resources[j].amount);

                    resources[i].amount += transfer;
                    resources[j].amount -= transfer;

                    if (resources[i].IsFull)
                        break;
                }
            }
        }

        // remove any now-empty slots
        resources.RemoveAll(s => s.IsEmpty);
    }

    public bool RemoveResource(ResourceType type, int amount)
    {
        int totalAvailable = GetResourceAmount(type);
        if (totalAvailable < amount)
        {
            Debug.Log($"❌ Not enough {type} to remove {amount}");
            return false;
        }

        int remaining = amount;

        foreach (var slot in resources)
        {
            if (slot.type == type && slot.amount > 0)
            {
                int toRemove = Mathf.Min(slot.amount, remaining);
                slot.amount -= toRemove;
                remaining -= toRemove;

                if (slot.amount <= 0) slot.amount = 0;
                if (remaining <= 0) break;
            }
        }

        // Remove empty slots at the end
        resources.RemoveAll(s => s.amount <= 0);
        AutoSortInventory();
        UpdateResourcesUIText();
        return true;
    }

    public bool RemoveAllResources(List<RecipeSO.Ingredient> reqIngredients)
    {
        if (HasAllResources(reqIngredients))
        {
            foreach (var pair in reqIngredients)
            {
                RemoveResource(pair.type, pair.amount);
            }
            Debug.Log("Multiple resource removal : TRUE");
            return true;
        }
        else
        {
            Debug.Log("Multiple resource removal : FALSE");
            return false;
        }


    }

    public void ClearInventory()
    {
        resources.Clear();
    }

    public int GetResourceAmount(ResourceType type)
    {
        int total = 0;
        foreach (var slot in resources)
        {
            if (slot.type == type)
                total += slot.amount;
        }
        return total;
    }

    public bool HasResource(ResourceType type, int amount)
    {
        int total = 0;

        foreach (var slot in resources)
        {
            if (slot.type == type)
            {
                total += slot.amount;
                if (total >= amount)
                    return true; // we already have enough
            }
        }

        return false; // not enough in total
    }

    public bool HasAllResources(List<RecipeSO.Ingredient> required)
    {
        foreach (var pair in required)
        {
            if(!HasResource(pair.type, pair.amount))
                return false;
        }
        return true;
    }

    
    void Update()
    {
        UpdateResourcesUIText();
    }

    void UpdateResourcesUIText()
    {
        /*for (int i = 0; i < images.Count; i++)
        {
            if (i < resources.Count && !resources[i].IsEmpty)
            {
                images[i].sprite = GetSpriteForType(resources[i].type);
                images[i].color = Color.white;
                inventoryNumberDisplay[i].text = resources[i].amount.ToString();
            }
            else
            {
                images[i].sprite = null;
                images[i].color = new Color(1, 1, 1, 0);
                inventoryNumberDisplay[i].text = "";
            }
        }*/

        // ensure no mismatch causes issues
        int slotCount = Mathf.Min(images.Count, inventoryNumberDisplay.Count);
        int resourceCount = resources.Count;

        for (int i = 0; i < slotCount; i++)
        {
            bool hasResource = i < resourceCount && resources[i] != null && !resources[i].IsEmpty;

            if (hasResource)
            {
                images[i].sprite = GetSpriteForType(resources[i].type);
                images[i].color = Color.white;
                inventoryNumberDisplay[i].text = resources[i].amount.ToString();
            }
            else
            {
                // clear slot if no resource or no matching UI element
                images[i].sprite = null;
                images[i].color = new Color(1, 1, 1, 0);
                inventoryNumberDisplay[i].text = "";
            }
        }
    }


    public Sprite GetSpriteForType(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood: return woodSprite;
            case ResourceType.Stone: return stoneSprite;
            case ResourceType.Metal: return metalSprite;
            case ResourceType.Coal: return coalSprite;
            case ResourceType.Water: return waterSprite;
            case ResourceType.Diesel: return dieselSprite;
            case ResourceType.RawMeat: return rawMeatSprite;
            case ResourceType.CookedMeat: return cookedMeatSprite;
            case ResourceType.Berries: return berriesSprite;
            case ResourceType.StoneAxe: return stoneAxeSprite;
            case ResourceType.StonePickaxe: return stonePickaxeSprite;
            case ResourceType.Sword: return swordSprite;
            case ResourceType.CrudeOil: return crudeOilSprite;
            default: return null;
        }
    }

    private void AutoSortInventory()
    {
        // 1️⃣ Merge same-type stacks
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].IsEmpty || resources[i].IsFull) continue;

            for (int j = i + 1; j < resources.Count; j++)
            {
                if (resources[j].type == resources[i].type && !resources[j].IsEmpty)
                {
                    int space = 128 - resources[i].amount;
                    int transfer = Mathf.Min(space, resources[j].amount);

                    resources[i].amount += transfer;
                    resources[j].amount -= transfer;

                    if (resources[i].IsFull)
                        break;
                }
            }
        }

        // 2️⃣ Remove empty slots
        resources.RemoveAll(s => s.IsEmpty);

        // 3️⃣ Sort by resource type name for neatness
        resources.Sort((a, b) => a.type.CompareTo(b.type));
    }
}