using System.Collections.Generic;
using PIW_assets.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PIW_assets.player_inventory
{
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

        void Awake() => Instance = this;

        public void AddResource(ResourceType type, int amount, int durability = 0)
        {
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
            Debug.Log($"[Inventory] Selected food: {consumables[currentFoodIndex].type}");
        }

        public void EatFood(ResourceType type)
        {
            if (RemoveResource(type, 1))
            {
                float nutrition = 20f;
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
                        return true;
                }
            }
            return false;
        }

        public bool HasAllResources(List<RecipeSO.Ingredient> required)
        {
            foreach (var pair in required)
            {
                if (!HasResource(pair.type, pair.amount))
                    return false;
            }
            return true;
        }

        public bool RemoveResource(ResourceType type, int amount)
        {
            int totalAvailable = GetResourceAmount(type);
            if (totalAvailable < amount)
                return false;

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

            resources.RemoveAll(s => s.amount <= 0);
            AutoSortInventory();
            UpdateResourcesUIText();
            return true;
        }

        private void AutoSortInventory()
        {
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

            resources.RemoveAll(s => s.IsEmpty);
            resources.Sort((a, b) => a.type.CompareTo(b.type));
        }

        void Update() => UpdateResourcesUIText();

        void UpdateResourcesUIText()
        {
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
                case ResourceType.Elucidator: return swordSprite;
                case ResourceType.DarkRepulser: return swordSprite;
                case ResourceType.LambentLight: return swordSprite;
                case ResourceType.CrudeOil: return crudeOilSprite;
                default: return null;
            }
        }
    }
}
