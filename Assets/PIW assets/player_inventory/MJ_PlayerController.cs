using UnityEngine;
using UnityEngine.InputSystem;

namespace PIW_assets.player_inventory
{
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

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (cameraTransform == null) cameraTransform = Camera.main.transform;

            cycleToolsAction.action.performed += OnCycleTools;
            cycleFoodAction.action.performed += OnCycleFood;
        }

        private void OnDestroy()
        {
            cycleToolsAction.action.performed -= OnCycleTools;
            cycleFoodAction.action.performed -= OnCycleFood;
        }

        private void OnCycleTools(InputAction.CallbackContext context)
        {
            if (MJ_PlayerInventory.Instance != null)
                MJ_PlayerInventory.Instance.CycleTools();
        }

        private void OnCycleFood(InputAction.CallbackContext context)
        {
            if (MJ_PlayerInventory.Instance != null)
            {
                MJ_PlayerInventory.Instance.CycleFood();
                var consumables = MJ_PlayerInventory.Instance.GetConsumablesList();
                if (consumables.Count > 0)
                {
                    // intent to use food can be added here later
                }
            }
        }

        void FixedUpdate()
        {
            Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = (forward * moveInput.y + right * moveInput.x).normalized;
            rb.linearVelocity = new Vector3(moveDir.x * _speed, rb.linearVelocity.y, moveDir.z * _speed);

            float turnInput = turnAction.action.ReadValue<Vector2>().x;
            if (Mathf.Abs(turnInput) > 0.1f)
            {
                transform.Rotate(0, turnInput * 100f * Time.fixedDeltaTime, 0);
            }
        }
    }
}
