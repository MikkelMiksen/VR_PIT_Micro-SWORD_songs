using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MJ_PlayerController : MonoBehaviour
{
    public static MJ_PlayerController Instance; 
    
    void Awake() => Instance = this;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private InputActionProperty moveAction;
    [SerializeField] private InputActionProperty turnAction;
    
    [Header("VR References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Input Actions")]
    [SerializeField] private InputActionProperty cycleToolsAction;
    [SerializeField] private InputActionProperty cycleFoodAction;
    
    private Rigidbody rb;

    /// <summary>
    /// Phase 1: Initialize player controller
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        cycleToolsAction.action.performed += OnCycleTools;
        cycleFoodAction.action.performed += OnCycleFood;
    }
    
    /// <summary>
    /// Phase 2: Clean up input actions
    /// </summary>
    private void OnDestroy()
    {
        cycleToolsAction.action.performed -= OnCycleTools;
        cycleFoodAction.action.performed -= OnCycleFood;
    }

    /// <summary>
    /// Cycles for the next HUD item in the inventory.
    /// </summary>
    /// <param name="context"></param>
    private void OnCycleTools(InputAction.CallbackContext context)
    {
        // Phase 3: Cycle through tools (B button)
        if (MJ_PlayerInventory.Instance != null)
        {
            MJ_PlayerInventory.Instance.CycleTools();
        }
    }

    /// <summary>
    /// Cycles for the next HUD item in the inventory.
    /// </summary>
    /// <param name="context"></param>
    private void OnCycleFood(InputAction.CallbackContext context)
    {
        // Phase 4: Cycle through food (Y button)
        if (MJ_PlayerInventory.Instance != null)
        {
            MJ_PlayerInventory.Instance.CycleFood();
            
            // TODO: Add logic to eat the selected food item in separate action.
            // For now, let's also eat it immediately on Y press
            // because there's no "Use" button defined yet.
            var consumables = MJ_PlayerInventory.Instance.GetConsumablesList();
            if (consumables.Count > 0)
            {
                // In a real game, this might be a separate "Eat" action.
            }
        }
    }

    /// <summary>
    /// Phase 5: Move the player
    /// </summary>
    void FixedUpdate()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        
        // Build movement direction relative to camera yaw
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * moveInput.y + right * moveInput.x).normalized;
        
        // Apply velocity
        rb.linearVelocity = new Vector3(moveDir.x * _speed, rb.linearVelocity.y, moveDir.z * _speed);
        
        // Handle rotation (Snap turn or smooth turn could be added here)
        float turnInput = turnAction.action.ReadValue<Vector2>().x;
        if (Mathf.Abs(turnInput) > 0.1f)
        {
            transform.Rotate(0, turnInput * 100f * Time.fixedDeltaTime, 0);
        }
    }
}