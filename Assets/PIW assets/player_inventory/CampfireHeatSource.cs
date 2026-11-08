using System.Collections.Generic;
using UnityEngine;

namespace PIW_assets.player_inventory
{
    /// <summary>Local warmth emitted by a placed survival-slice campfire.</summary>
    public sealed class CampfireHeatSource : MonoBehaviour
    {
        private static readonly List<CampfireHeatSource> ActiveSources = new();

        [SerializeField, Min(0.1f)] private float warmthRadius = 5f;
        [SerializeField] private float warmthTemperature = 42f;

        private void OnEnable()
        {
            if (!ActiveSources.Contains(this)) ActiveSources.Add(this);
        }

        private void OnDisable() => ActiveSources.Remove(this);
        private void OnDestroy() => ActiveSources.Remove(this);

        public static float GetLocalTemperature(Vector3 position, float ambientTemperature)
        {
            float result = ambientTemperature;
            for (int i = ActiveSources.Count - 1; i >= 0; i--)
            {
                CampfireHeatSource source = ActiveSources[i];
                if (source == null)
                {
                    ActiveSources.RemoveAt(i);
                    continue;
                }

                float distance = Vector3.Distance(position, source.transform.position);
                if (distance >= source.warmthRadius) continue;

                float influence = 1f - distance / source.warmthRadius;
                result = Mathf.Max(result, Mathf.Lerp(ambientTemperature, source.warmthTemperature, influence));
            }
            return result;
        }
    }
}
