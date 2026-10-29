using UnityEngine;

public class SwordInteraction : MonoBehaviour
{
    [Header("Sword Data")]
    public SwordData swordData;

    [Header("Meta Interaction Settings")]
    public Transform leftHand;
    public Transform rightHand;
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private Rigidbody _rb;

    [Header("Swing Metrics")]
    [SerializeField] private SwordSwingTelemetry _telemetry = new SwordSwingTelemetry();
    [SerializeField] private SwordDamageMath _damageMath = new SwordDamageMath();
    [SerializeField] private SwordLoggingManager _loggingManager = new SwordLoggingManager();

    private bool _isEquipped = false;

    public float AverageSwingSpeed => _telemetry != null ? _telemetry.AverageSpeed : 0f;
    public SwordDamageMath DamageMath => _damageMath;
    public SwordLoggingManager LoggingManager => _loggingManager;

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
        if (_telemetry == null) _telemetry = new SwordSwingTelemetry();
        if (_damageMath == null) _damageMath = new SwordDamageMath();
        if (_loggingManager == null) _loggingManager = new SwordLoggingManager();
    }

    void Update()
    {
        _isEquipped = IsEquipped;
        _telemetry?.Sample(transform.position, Time.deltaTime);
    }

    public float CalculateSwingDamage(float baseDamage, float armor)
    {
        if (_damageMath == null) return baseDamage;
        return _damageMath.CalculateDamage(baseDamage, AverageSwingSpeed, armor);
    }

    public Vector3 CalculateSwingKnockback(Vector3 hitDirection, float baseKnockback)
    {
        if (_damageMath == null) return Vector3.zero;
        return _damageMath.CalculateKnockback(AverageSwingSpeed, hitDirection, baseKnockback);
    }

    public void LogSwing(float rawDamage, float finalDamage, float armorValue, Vector3 hitDirection, Vector3 knockback)
    {
        if (_loggingManager == null) return;
        _loggingManager.LogSwing(swordData != null ? swordData.swordName : "Sword", AverageSwingSpeed, rawDamage, finalDamage, armorValue, hitDirection, knockback);
    }

    public void Interact()
    {
    }

    public bool IsSheathed => false;

    public void Drop()
    {
        if (_grabbable != null)
        {
        }
    }
}
