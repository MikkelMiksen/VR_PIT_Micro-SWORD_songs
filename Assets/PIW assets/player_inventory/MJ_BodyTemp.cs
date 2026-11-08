using UnityEngine;
using PIW_assets.Sword_interaction;
using PIW_assets.worldBehaviour;

namespace PIW_assets.player_inventory
{
    public class MJ_BodyTemp : MonoBehaviour
    {
        public static MJ_BodyTemp Instance;

        [Header("Body temperature")]
        public float bodyTemp = 37f;
        public float minSafeBodyTemp = 28f;
        public float maxSafeBodyTemp = 41f;
        public float temperatureChangeRate = 0.02f;
        public float currentAreaTemp = 10f;
        public bool isBodyTempSafe, isBodyTempFreezing, isBodyTempOverheating;
        public float hpInfluenceCoefficient;
        public string readBodyTemp;

        private void Awake() => Instance = this;

        private void Start()
        {
            if (DayNightCycle.Instance != null) currentAreaTemp = DayNightCycle.Instance.temp;
            CheckTemperatureEffects();
        }

        private void Update()
        {
            if (DayNightCycle.Instance != null) currentAreaTemp = DayNightCycle.Instance.temp;
            float localTemperature = CampfireHeatSource.GetLocalTemperature(transform.position, currentAreaTemp);
            float delta = localTemperature - bodyTemp;
            float adjustment = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta), 1.3f);
            bodyTemp = Mathf.Clamp(bodyTemp + adjustment * temperatureChangeRate * Time.deltaTime, 19.8f, 45f);
            CheckTemperatureEffects();

            if (PlayerStats.Instance != null && !isBodyTempSafe)
                PlayerStats.Instance.TakeDamage(2f * Time.deltaTime);
        }

        private void CheckTemperatureEffects()
        {
            isBodyTempSafe = bodyTemp > minSafeBodyTemp && bodyTemp < maxSafeBodyTemp;
            isBodyTempFreezing = bodyTemp <= minSafeBodyTemp;
            isBodyTempOverheating = bodyTemp >= maxSafeBodyTemp;
            hpInfluenceCoefficient = isBodyTempSafe ? 1f : 0.96f;
            readBodyTemp = isBodyTempFreezing
                ? $"Hypothermia risk! Body temp: {bodyTemp:F1}°C"
                : isBodyTempOverheating
                    ? $"Overheating risk! Body temp: {bodyTemp:F1}°C"
                    : $"Body temp: {bodyTemp:F1}°C";
        }
    }
}
