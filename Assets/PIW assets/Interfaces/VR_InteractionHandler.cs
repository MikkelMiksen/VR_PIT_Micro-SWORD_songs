using UnityEngine;
using UnityEngine.InputSystem;

public class VR_InteractionHandler : MonoBehaviour
{
    [SerializeField] private InputActionProperty interactAction;
    [SerializeField] private float interactDistance = 3.0f;
    [SerializeField] private LayerMask interactableLayer;

    void Update()
    {
        if (interactAction.action.WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
        {
            Iinteractable interactable = hit.collider.GetComponent<Iinteractable>();
            if (interactable == null)
            {
                interactable = hit.collider.GetComponentInParent<Iinteractable>();
            }

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
