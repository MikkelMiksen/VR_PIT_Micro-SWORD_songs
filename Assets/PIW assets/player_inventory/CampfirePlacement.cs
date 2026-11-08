using UnityEngine;
using UnityEngine.InputSystem;

namespace PIW_assets.player_inventory
{
    /// <summary>Places a simple fire a short distance in front of the player after spending wood.</summary>
    public sealed class CampfirePlacement : MonoBehaviour
    {
        [SerializeField] private InputActionProperty placeAction;
        [SerializeField] private Transform placementOrigin;
        [SerializeField] private GameObject campfirePrefab;
        [SerializeField, Min(0)] private int woodCost = 3;
        [SerializeField, Min(0.5f)] private float placementDistance = 2f;
        [SerializeField] private LayerMask groundLayers = ~0;

        private void OnEnable()
        {
            if (placeAction.action != null) placeAction.action.Enable();
        }

        private void OnDisable()
        {
            if (placeAction.action != null) placeAction.action.Disable();
        }

        private void Update()
        {
            if (placeAction.action == null || !placeAction.action.WasPressedThisFrame()) return;
            PlaceCampfire();
        }

        public void PlaceCampfire()
        {
            MJ_PlayerInventory inventory = MJ_PlayerInventory.Instance;
            if (inventory == null)
            {
                Debug.LogWarning("Cannot place campfire: player inventory is missing.");
                return;
            }
            if (!inventory.HasResource(PIW_assets.Resources.ResourceType.Wood, woodCost))
            {
                Debug.Log("Gather more wood before placing a campfire.");
                return;
            }

            Transform origin = placementOrigin != null ? placementOrigin : transform;
            Vector3 position = origin.position + origin.forward * placementDistance;
            Vector3 rayStart = position + Vector3.up * 3f;
            if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f, groundLayers, QueryTriggerInteraction.Ignore))
                position = hit.point;
            else
                position.y = transform.position.y;

            if (!inventory.RemoveResource(PIW_assets.Resources.ResourceType.Wood, woodCost)) return;

            GameObject fire = campfirePrefab != null ? Instantiate(campfirePrefab, position, Quaternion.identity) : CreateFallbackFire(position);
            if (fire.GetComponent<CampfireHeatSource>() == null) fire.AddComponent<CampfireHeatSource>();
        }

        private static GameObject CreateFallbackFire(Vector3 position)
        {
            GameObject root = new GameObject("Placed Campfire");
            root.transform.position = position;

            GameObject logs = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            logs.name = "Firewood";
            logs.transform.SetParent(root.transform, false);
            logs.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            logs.transform.localScale = new Vector3(0.5f, 0.08f, 0.5f);

            GameObject flame = new GameObject("Warm Light");
            flame.transform.SetParent(root.transform, false);
            flame.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            Light light = flame.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = new Color(1f, 0.48f, 0.12f);
            light.intensity = 2f;
            light.range = 5f;
            light.shadows = LightShadows.None;
            return root;
        }
    }
}
