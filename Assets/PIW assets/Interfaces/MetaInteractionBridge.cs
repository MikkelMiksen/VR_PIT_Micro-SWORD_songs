using UnityEngine;
using Oculus.Interaction;

/// <summary>
/// Bridges Meta Interaction SDK events (Ray/Grab) to the legacy Iinteractable system.
/// Attach this to any object with an Iinteractable component.
/// </summary>
public class MetaInteractionBridge : MonoBehaviour
{
    private Iinteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<Iinteractable>();
        if (_interactable == null)
        {
            _interactable = GetComponentInParent<Iinteractable>();
        }
    }

    /// <summary>
    /// Call this from a Meta Interaction SDK Interactable (e.g., PointableUnityEventWrapper or InteractableUnityEventWrapper)
    /// </summary>
    public void OnInteract()
    {
        if (_interactable != null)
        {
            _interactable.Interact();
        }
    }

    /// <summary>
    /// Logic for handling Grab-based interaction if different from Ray
    /// </summary>
    public void OnGrab()
    {
        OnInteract();
    }
}
