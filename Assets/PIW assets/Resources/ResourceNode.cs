using UnityEngine;
using PIW_assets.player_inventory;

namespace PIW_assets.Resources
{
    /// <summary>World object that adds its configured resources when selected through VR interaction.</summary>
    public class ResourceNode : MonoBehaviour, Iinteractable
    {
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;
        [SerializeField, Min(1)] private int resourceAmount = 1;
        [SerializeField] private bool shouldDestroyResourceOnCollection = true;
        private bool collected;

        public void Interact()
        {
            if (collected || MJ_PlayerInventory.Instance == null || resourceAmount <= 0) return;
            MJ_PlayerInventory.Instance.AddResource(resourceType, resourceAmount);
            collected = true;
            if (shouldDestroyResourceOnCollection) Destroy(gameObject);
        }

        public string GetPrompt() => $"Select to gather {resourceType}";
    }
}
