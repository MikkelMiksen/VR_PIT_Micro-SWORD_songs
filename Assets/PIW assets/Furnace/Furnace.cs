using System.Collections.Generic;
using UnityEngine;
using PIW_assets.Resources;
using PIW_assets.player_inventory;

namespace PIW_assets.Furnace
{
    public class Furnace : MonoBehaviour, Iinteractable
    {
        public static Furnace Instance;
        public FurnaceUI furnaceUI;
        private bool isUIOpen = false;

        private Dictionary<ResourceType, int> fuels = new Dictionary<ResourceType, int>();
        public bool hasFuel => fuels.Count > 0;
        public bool furnaceIsOn = false;

        private ResourceType currentFuelType;
        private bool isBurningFuel = false;

        private float burnTimer = 0f;
        private float currentUnitBurnTime = 0f;
        public float totalHeatOutput = 0f;
        private Material fireMat;

        [SerializeField] private float influenceRadius = 25f, heatMultiplier = 1f;
        public float temperatureInfluence = 0;
        [SerializeField] private Light flameLight;

        void Awake() => Instance = this;

        void Start() { fuels.Add(ResourceType.Diesel, 10); }

        public void Interact()
        {
            if (!isUIOpen)
            {
                furnaceUI.Show();
                isUIOpen = true;
            }
            else
            {
                furnaceUI.Hide();
                isUIOpen = false;
            }
        }

        public void AddFuel(ResourceType type, int amount)
        {
            if (!IsBurnable(type))
            {
                Debug.Log($"❌ {type} cannot be burned!");
                return;
            }

            if (MJ_PlayerInventory.Instance.RemoveResource(type, amount))
            {
                if (!fuels.ContainsKey(type))
                    fuels[type] = 0;

                fuels[type] += amount;
                furnaceIsOn = true;
            }
            else
            {
                Debug.Log($"❌ Not enough {type} in inventory!");
            }
        }

        private bool IsBurnable(ResourceType type)
        {
            return ResourceData.BurnProperties[type].IsBurnable;
        }

        public string GetPrompt() => "Press E to Interact";

        public string GetResourceAmount(ResourceType type)
        {
            return fuels.ContainsKey(type) ? fuels[type].ToString() : "0";
        }

        void Update()
        {
            if (!furnaceIsOn || !hasFuel) return;

            if (!isBurningFuel)
            {
                foreach (var kvp in fuels)
                {
                    currentFuelType = kvp.Key;
                    currentUnitBurnTime = ResourceData.BurnProperties[currentFuelType].BurnTime / ResourceData.BurnProperties[currentFuelType].BurnRate;
                    burnTimer = 0f;
                    isBurningFuel = true;
                    break;
                }
            }

            if (isBurningFuel)
            {
                burnTimer += Time.deltaTime;
                totalHeatOutput = ResourceData.BurnProperties[currentFuelType].HeatOutput * heatMultiplier;

                if (burnTimer >= currentUnitBurnTime)
                {
                    burnTimer = 0f;
                    fuels[currentFuelType]--;
                    if (fuels[currentFuelType] <= 0)
                        fuels.Remove(currentFuelType);
                    isBurningFuel = false;
                }
            }

            if (!hasFuel)
            {
                furnaceIsOn = false;
                totalHeatOutput = 0f;
            }

            GameObject fire = GameObject.Find("Fire");
            if (isBurningFuel && fire != null)
            {
                temperatureInfluence = (influenceRadius - Vector3.Distance(MJ_PlayerController.Instance.transform.position, transform.position)) / influenceRadius;
                flameLight.intensity = 25.02f;
                Color tmpCol = fire.GetComponent<MeshRenderer>().material.color;
                tmpCol.a = 255;
                fire.GetComponent<MeshRenderer>().material.color = tmpCol;
            }
            else if (fire != null)
            {
                temperatureInfluence = 0f;
                flameLight.intensity = 0;
                Color tmpCol = fire.GetComponent<MeshRenderer>().material.color;
                tmpCol.a = 0;
                fire.GetComponent<MeshRenderer>().material.color = tmpCol;
            }
        }
    }
}
