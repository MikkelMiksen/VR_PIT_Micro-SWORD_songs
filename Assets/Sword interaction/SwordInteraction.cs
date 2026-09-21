using UnityEngine;
using Oculus.Interaction;

public class SwordInteraction : MonoBehaviour
{
    [Header("Sword Data")]
    public SwordData swordData;

    [Header("Meta Interaction Settings")]
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private Rigidbody _rb;

    private bool _isEquipped = false;

    public bool IsEquipped
    {
        get
        {
            if (_grabbable != null)
                return _grabbable.SelectingPointsCount > 0;
            return _isEquipped;
        }
    }

    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_grabbable == null) _grabbable = GetComponent<Grabbable>();
    }

    void Update()
    {
        // Meta Interaction SDK handles positioning when grabbed
        _isEquipped = IsEquipped;
    }

    public void Interact()
    {
        // For MetaInteractionBridge compatibility
    }

    public bool IsSheathed => false;

    // Helper to drop the sword if needed by script
    public void Drop()
    {
        if (_grabbable != null)
        {
            // Meta Interaction SDK doesn't have a direct "ForceDrop" on Grabbable easily 
            // without accessing the Interactor, but we can disable/enable it.
        }
    }
}
