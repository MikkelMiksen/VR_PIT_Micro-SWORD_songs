using UnityEngine;

public class SwordDamageTrigger : MonoBehaviour
{
    private SwordInteraction _sword;

    private void Start()
    {
        _sword = GetComponentInParent<SwordInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_sword == null || !_sword.IsEquipped)
        {
            return;
        }

        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        var enemyStats = other.GetComponent<EnemyStats>();
        if (enemyStats == null) enemyStats = other.GetComponentInParent<EnemyStats>();
        if (enemyStats == null) return;

        float swingSpeed = _sword.AverageSwingSpeed;
        if (_sword.HitGuard != null && !_sword.HitGuard.CanHit(other.gameObject, swingSpeed))
        {
            return;
        }

        float baseDamage = _sword.swordData != null ? _sword.swordData.damage : (PlayerStats.Instance != null ? PlayerStats.Instance.swordDamage : 25f);
        float cost = _sword.swordData != null ? _sword.swordData.staminaCost : (PlayerStats.Instance != null ? PlayerStats.Instance.attackStaminaCost : 20f);

        if (PlayerStats.Instance != null && !PlayerStats.Instance.CanAttack(cost))
        {
            return;
        }

        Vector3 hitDirection = other.transform.position - _sword.transform.position;
        if (hitDirection.sqrMagnitude < 0.0001f)
        {
            hitDirection = _sword.transform.forward;
        }

        float finalDamage = _sword.CalculateSwingDamage(baseDamage, enemyStats.armor);
        Vector3 knockback = _sword.CalculateSwingKnockback(hitDirection, _sword.swordData != null ? _sword.swordData.knockbackForce : 4f);

        enemyStats.TakeDamage(finalDamage, hitDirection, swingSpeed);
        enemyStats.ApplyKnockback(knockback);

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.UseStamina(cost);
        }

        _sword.LogSwing(baseDamage, finalDamage, enemyStats.armor, hitDirection, knockback);

        Debug.Log($"[SAO] Enemy hit for {finalDamage:F2} damage with {_sword?.swordData?.swordName ?? "Blade"}! swingSpeed={swingSpeed:F2}");
    }
}
