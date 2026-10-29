using UnityEngine;

public class SwordDamageMath
{
    public float ArmorMitigation(float armor, float armorK = 100f)
    {
        return armor / (armor + Mathf.Max(armorK, 1f));
    }

    public float CalculateDamage(float incomingDamage, float averageSwingSpeed, float armor, float armorK = 100f, float speedReference = 12f, float speedMultiplier = 1.5f)
    {
        float speedFactor = Mathf.Clamp01(averageSwingSpeed / speedReference);
        float mitigation = ArmorMitigation(armor, armorK);
        float adjustedDamage = incomingDamage * (1f + speedFactor * speedMultiplier);

        return Mathf.Max(0f, adjustedDamage * (1f - mitigation));
    }

    public Vector3 CalculateKnockback(float averageSwingSpeed, Vector3 hitDirection, float baseKnockback, float speedReference = 12f)
    {
        if (hitDirection.sqrMagnitude < 0.0001f)
        {
            hitDirection = Vector3.forward;
        }

        float forceFactor = Mathf.Clamp(averageSwingSpeed / speedReference, 0f, 2.5f);
        return hitDirection.normalized * (baseKnockback * (1f + forceFactor));
    }
}
