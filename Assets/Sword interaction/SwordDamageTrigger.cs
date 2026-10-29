using UnityEngine;

namespace PIW_assets.Sword_interaction
{
    public class SwordDamageTrigger : MonoBehaviour
    {
        private SwordInteraction _sword;

        private void Start() => _sword = GetComponentInParent<SwordInteraction>();

        private void OnTriggerEnter(Collider other)
        {
            if (_sword == null || !_sword.IsEquipped) return;
            if (!other.CompareTag("Enemy")) return;

            var enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats == null) enemyStats = other.GetComponentInParent<EnemyStats>();
            if (enemyStats == null) return;

            float damage = _sword.swordData != null ? _sword.swordData.damage : (PlayerStats.Instance != null ? PlayerStats.Instance.swordDamage : 25f);
            float cost = _sword.swordData != null ? _sword.swordData.staminaCost : (PlayerStats.Instance != null ? PlayerStats.Instance.attackStaminaCost : 20f);

            if (PlayerStats.Instance != null)
            {
                if (PlayerStats.Instance.CanAttack(cost))
                {
                    enemyStats.TakeDamage(damage);
                    PlayerStats.Instance.UseStamina(cost);
                }
            }
            else
            {
                enemyStats.TakeDamage(damage);
            }
        }
    }
}
