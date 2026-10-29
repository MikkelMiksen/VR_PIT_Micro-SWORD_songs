using UnityEngine;
using PIW_assets.player_inventory;

namespace PIW_assets.Resources
{
    public class ResourceNode : MonoBehaviour, Iinteractable
    {
        [SerializeField] private ResourceType resourceType = ResourceType.Wood;
        [SerializeField] private int resourceAmount;
        [SerializeField] private bool shouldDestroyResourceOnCollection;

        public void Interact()
        {
            MJ_PlayerInventory.Instance.AddResource(resourceType, resourceAmount);
            if (shouldDestroyResourceOnCollection)
            {
                Destroy(gameObject);
            }
        }

        public string GetPrompt() => "Press E to gather " + resourceType;
    }
}
