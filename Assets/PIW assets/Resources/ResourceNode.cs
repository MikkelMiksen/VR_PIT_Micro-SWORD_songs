using UnityEngine;

namespace PIW_assets.Resources
{
    /// <summary>
    /// Use this class to make any object in the scene a resource node.
    /// </summary>
    public class ResourceNode : MonoBehaviour, Iinteractable
    {
        [SerializeField] ResourceType resourceType = ResourceType.Wood;

        [SerializeField] private int resourceAmount;

        [SerializeField] private bool shouldDestroyResourceOnCollection;

        /// <summary>
        /// Collects resource node from the pile and adds it to the player inventory.
        /// </summary>
        public void Interact()
        {
            MJ_PlayerInventory.Instance.AddResource(resourceType, resourceAmount);
            if (shouldDestroyResourceOnCollection)
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Returns the prompt for the resource node pile.
        /// </summary>
        /// <returns></returns>
        public string GetPrompt() => "Press E to gather " + resourceType;
    }
}