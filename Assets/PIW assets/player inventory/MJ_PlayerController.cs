using UnityEngine;
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
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
    }

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