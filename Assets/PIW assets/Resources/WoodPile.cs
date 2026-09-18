using UnityEngine;
using UnityEngine.EventSystems;

public class WoodPile : MonoBehaviour, Iinteractable
{

    [SerializeField]
    private int woodAmount;

    [SerializeField]
    private bool shouldDestroyResourceOnCollection;

    public void Interact()
    {
        MJ_PlayerInventory.Instance.AddResource(ResourceType.Wood, woodAmount);
        if (shouldDestroyResourceOnCollection)
        {
            Destroy(gameObject);
        }
    }

    public string GetPrompt() => "Press E to gather wood";
}
