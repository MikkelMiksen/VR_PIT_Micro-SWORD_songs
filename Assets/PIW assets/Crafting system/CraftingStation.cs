using UnityEngine;

public abstract class CraftingStation : MonoBehaviour, Iinteractable
{
    public abstract WorkstationType StationType { get; }
    public abstract string StationName { get; }

    public virtual void Interact()
    {
        // Opens the UI for this station
        //CraftingUIController.Instance.OpenForStation(this);
    }

    public virtual string GetPrompt()
    {
        return $"Use {StationName}";
    }
}