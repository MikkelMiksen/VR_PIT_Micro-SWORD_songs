using UnityEngine;
using UnityEngine.UI;

namespace PIW_assets.Sword_interaction
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 100f;
        public float currentHealth;

        [Header("Stamina")]
        public float maxStamina = 100f;
        public float currentStamina;
        public float staminaRegenRate = 10f;

        [Header("Combat Settings")]
        public float attackStaminaCost = 20f;
        public float swordDamage = 25f;

        public PhysicsMaterial playerPhysicsMaterial;
        public int score = 0;
        public static PlayerStats Instance { get; private set; }

        private void Awake() => Instance ??= this;

        private void Start()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;

            if (playerPhysicsMaterial != null)
            {
                var colliders = GetComponentsInChildren<Collider>();
                foreach (var col in colliders)
                    col.material = playerPhysicsMaterial;
            }
        }

        private void Update()
        {
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                currentStamina = Mathf.Min(currentStamina, maxStamina);
            }
        }

        public void AddScore(int amount) => score += amount;
        public bool CanAttack(float cost) => currentStamina >= cost;
        public void UseStamina(float amount)
        {
            currentStamina -= amount;
            currentStamina = Mathf.Max(currentStamina, 0);
        }
        public void ResetStats() { currentHealth = maxHealth; currentStamina = maxStamina; score = 0; }
        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Max(currentHealth, 0);
            if (currentHealth <= 0)
                Debug.Log("[SAO] Player Died!");
        }
    }
}
