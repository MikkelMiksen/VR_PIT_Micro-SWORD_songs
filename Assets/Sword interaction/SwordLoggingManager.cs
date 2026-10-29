using UnityEngine;

public class SwordLoggingManager
{
    public void LogSwing(string swordName, float averageSwingSpeed, float rawDamage, float finalDamage, float armorValue, Vector3 hitDirection, Vector3 knockback)
    {
        Debug.Log(
            $"[SwordLogging] {swordName} avgSpeed={averageSwingSpeed:F2} rawDamage={rawDamage:F2} finalDamage={finalDamage:F2} armor={armorValue:F2} hitDir={hitDirection.normalized} knockback={knockback.magnitude:F2}"
        );
    }
}
