using UnityEngine;
using UnityEngine.EventSystems;

public class CoalPile : MonoBehaviour, Iinteractable
{
    [SerializeField]
    private int coalAmount;

    [SerializeField]
    private bool shouldDestroyResourceOnCollection;

    public void Interact()
    {
        MJ_PlayerInventory.Instance.AddResource(ResourceType.Coal, coalAmount);
        if (shouldDestroyResourceOnCollection)
        {
            Destroy(gameObject);
        }
    }

    public string GetPrompt() => "Press E to gather coal";
}
