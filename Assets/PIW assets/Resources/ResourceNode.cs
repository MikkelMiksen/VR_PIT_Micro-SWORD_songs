using UnityEngine;
using PIW_assets.player_inventory;

namespace PIW_assets.Resources
{
    /// <summary>
    /// Use this class to make any object in the scene a resource node.
    /// </summary>
    public class ResourceNode : MonoBehaviour, Iinteractable
    {
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;
        [SerializeField] private int resourceAmount;
        [SerializeField] private bool shouldDestroyResourceOnCollection;

        public void Interact()
        {
            if (MJ_PlayerInventory.Instance != null)
            {
                MJ_PlayerInventory.Instance.AddResource(resourceType, resourceAmount);
            }

            if (shouldDestroyResourceOnCollection)
            {
                Destroy(gameObject);
            }
        }

        public string GetPrompt() => "Press E to gather " + resourceType;
    }
}
