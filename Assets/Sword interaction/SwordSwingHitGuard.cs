using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.Sword_interaction
{
    public class SwordSwingHitGuard : MonoBehaviour
    {
        [SerializeField] private float hitCooldown = 0.18f;
        [SerializeField] private float minimumSwingSpeedForHit = 0.8f;

        private readonly Dictionary<int, float> _lastHitTimes = new Dictionary<int, float>();

        public bool CanHit(GameObject enemy, float swingSpeed)
        {
            if (enemy == null) return false;
            if (swingSpeed < minimumSwingSpeedForHit) return false;

            int id = enemy.GetInstanceID();
            if (_lastHitTimes.TryGetValue(id, out float hitTime) && Time.time - hitTime < hitCooldown)
                return false;

            _lastHitTimes[id] = Time.time;
            return true;
        }

        public void ClearEnemy(GameObject enemy)
        {
            if (enemy == null) return;
            _lastHitTimes.Remove(enemy.GetInstanceID());
        }
    }
}
