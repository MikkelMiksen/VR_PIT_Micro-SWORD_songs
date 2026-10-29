using UnityEngine;
using PIW_assets.Sword_interaction;
using PIW_assets.worldBehaviour;

namespace PIW_assets.player_inventory
{
    public class MJ_BodyTemp : MonoBehaviour
    {
        public static MJ_BodyTemp Instance;

        void Awake()
        {
            Instance = this;
        }

        public float bodyTemp = 37f;
        public float minSafeBodyTemp = 28f;
        public float maxSafeBodyTemp = 41f;
        public float temperatureChangeRate = 0.02f;
        public float currentAreaTemp;
        public bool isBodyTempSafe, isBodyTempFreezing, isBodyTempOverheating;
        public float hpInfluenceCoefficient;
        public string readBodyTemp;

        void Start()
        {
            currentAreaTemp = DayNightCycle.Instance.temp;
        }

        void Update()
        {
            UpdateBodyTemperature();
            CheckTemperatureEffects();
            UpdateCurrentAreaTemp();
            ApplyTemperatureDamage();
        }

        private void ApplyTemperatureDamage()
        {
            if (PlayerStats.Instance != null && !isBodyTempSafe)
            {
                float damage = 2f * Time.deltaTime;
                PlayerStats.Instance.TakeDamage(damage);
            }
        }

        private void UpdateCurrentAreaTemp()
        {
            if (DayNightCycle.Instance != null)
            {
                currentAreaTemp = DayNightCycle.Instance.temp;
            }
        }

        private void UpdateBodyTemperature()
        {
            float environmentTemp = currentAreaTemp;
            float delta = environmentTemp - bodyTemp;
            float adjustment = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta), 1.3f);
            bodyTemp += adjustment * temperatureChangeRate * Time.deltaTime;
            bodyTemp = Mathf.Clamp(bodyTemp, 19.8f, 45f);
        }

        private void CheckTemperatureEffects()
        {
            if (bodyTemp >= minSafeBodyTemp && bodyTemp <= maxSafeBodyTemp)
            {
                readBodyTemp = $"Body temp: {bodyTemp:F1}°C";
                isBodyTempSafe = true;
                isBodyTempFreezing = false;
                isBodyTempOverheating = false;
                hpInfluenceCoefficient = 1;
            }
            else if (bodyTemp <= minSafeBodyTemp)
            {
                readBodyTemp = $"Hypothermia risk! Body temp: {bodyTemp:F1}°C";
                isBodyTempSafe = false;
                isBodyTempFreezing = true;
                isBodyTempOverheating = false;
                hpInfluenceCoefficient = 0.96f;
            }
            else if (bodyTemp >= maxSafeBodyTemp)
            {
                readBodyTemp = $"Overheating risk! Body temp: {bodyTemp:F1}°C";
                isBodyTempSafe = false;
                isBodyTempFreezing = false;
                isBodyTempOverheating = true;
                hpInfluenceCoefficient = 0.96f;
            }
        }
    }
}
