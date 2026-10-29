using UnityEngine;
using PIW_assets.Resources;
using PIW_assets.player_inventory;

namespace PIW_assets.Sword_interaction
{
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
        [SerializeField] private SwordSwingTelemetry _telemetry;
        [SerializeField] private SwordDamageMath _damageMath;
        [SerializeField] private SwordLoggingManager _loggingManager;
        [SerializeField] private SwordSwingHitGuard _hitGuard;

        private bool _isEquipped = false;

        public float AverageSwingSpeed => _telemetry != null ? _telemetry.AverageSpeed : 0f;
        public SwordDamageMath DamageMath => _damageMath;
        public SwordLoggingManager LoggingManager => _loggingManager;
        public SwordSwingHitGuard HitGuard => _hitGuard;

        public bool IsEquipped
        {
            get
            {
                if (_grabbable != null)
                    return _grabbable.SelectingPointsCount > 0;
                return _isEquipped;
            }
        }

        private void Awake()
        {
            if (_rb == null) _rb = GetComponent<Rigidbody>();
            if (_grabbable == null) _grabbable = GetComponent<Grabbable>();
            if (_telemetry == null) _telemetry = new SwordSwingTelemetry();
            if (_damageMath == null) _damageMath = new SwordDamageMath();
            if (_loggingManager == null) _loggingManager = new SwordLoggingManager();
            if (_hitGuard == null) _hitGuard = GetComponent<SwordSwingHitGuard>();
            if (_hitGuard == null) _hitGuard = gameObject.AddComponent<SwordSwingHitGuard>();
        }

        private void Update()
        {
            _isEquipped = IsEquipped;
            if (_telemetry != null)
                _telemetry.Sample(transform.position, Time.deltaTime);
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

        public void Interact() { }
        public bool IsSheathed => false;
        public void Drop() { }
    }
}
