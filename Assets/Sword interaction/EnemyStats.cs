using UnityEngine;
using UnityEngine.UI;

namespace PIW_assets.Sword_interaction
{
    public class EnemyStats : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 50f;
        public float currentHealth;

        [Header("Armor")]
        public float armor = 25f;
        public float armorK = 100f;

        [Header("UI")]
        public Image healthBarFill;
        public Canvas healthBarCanvas;

        public bool IsDead => currentHealth <= 0;

        private Animator _animator;
        private bool _isDying = false;
        private Rigidbody _rb;

        private void Start()
        {
            currentHealth = maxHealth;
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            UpdateUI();
        }

        private void Update()
        {
            if (healthBarCanvas != null && Camera.main != null)
            {
                healthBarCanvas.transform.LookAt(Camera.main.transform);
                healthBarCanvas.transform.Rotate(0, 180, 0);
            }
        }

        public void ApplyKnockback(Vector3 force)
        {
            if (_rb != null)
                _rb.AddForce(force, ForceMode.Impulse);
        }

        public void TakeDamage(float amount)
        {
            TakeDamage(amount, Vector3.zero, 0f);
        }

        public void TakeDamage(float amount, Vector3 hitDirection, float swingSpeed)
        {
            if (_isDying) return;

            float mitigation = armor / (armor + armorK);
            float finalDamage = Mathf.Max(0f, amount * (1f - mitigation));

            currentHealth -= finalDamage;
            currentHealth = Mathf.Max(currentHealth, 0);
            UpdateUI();

            if (currentHealth > 0)
            {
                if (_animator != null && _animator.HasState(0, Animator.StringToHash("GetHit")))
                    _animator.CrossFade("GetHit", 0.1f);
            }
            else
            {
                if (PlayerStats.Instance != null)
                    PlayerStats.Instance.AddScore(100);
                Die();
            }
        }

        private void UpdateUI()
        {
            if (healthBarFill != null)
                healthBarFill.fillAmount = currentHealth / maxHealth;
        }

        private void Die()
        {
            if (_isDying) return;
            _isDying = true;

            MonsterSpawner spawner = FindFirstObjectByType<MonsterSpawner>();
            if (spawner != null)
                spawner.DecrementCount();

            var colliders = GetComponentsInChildren<Collider>();
            foreach (var col in colliders)
                col.enabled = false;

            if (healthBarCanvas != null)
                healthBarCanvas.gameObject.SetActive(false);

            if (_animator != null && _animator.HasState(0, Animator.StringToHash("Die")))
            {
                _animator.CrossFade("Die", 0.1f);
                Destroy(gameObject, 1.5f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
