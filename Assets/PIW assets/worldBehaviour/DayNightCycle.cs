using System;
using TMPro;
using UnityEngine;

namespace PIW_assets.worldBehaviour
{
    [System.Serializable]
    public struct SeasonTemperature
    {
        public string name;
        public float avgHigh;
        public float avgLow;
    }

    public class DayNightCycle : MonoBehaviour
    {
        [Header("Read Temperature")] public float temp;
        public static DayNightCycle Instance;

        void Awake() => Instance = this;

        [Header("Time attributes")]
        [SerializeField] private float timeMultiplier, startHour, sunriseHour, sunsetHour, maxSunLightIntensity, maxMoonLightIntensity;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Light sunLight, moonLight;
        [SerializeField] private Color dayAmbientLight, nightAmbientLight;
        [SerializeField] private AnimationCurve lightChangeCurve;

        public DateTime currentTime;
        private TimeSpan sunriseTime, sunsetTime;

        [Header("Temperature attributes")]
        [SerializeField] private SeasonTemperature[] seasons;
        [SerializeField] private float currentTemperature;
        [SerializeField] private AnimationCurve dailyTemperatureCurve;
        [SerializeField] private TextMeshProUGUI temperatureText;
        [SerializeField] private AnimationCurve yearlyTemperatureCurve;

        void Start()
        {
            currentTime = DateTime.Now.AddYears(-71);
            currentTime = currentTime.Date + TimeSpan.FromHours(startHour);
            sunriseTime = TimeSpan.FromHours(sunriseHour);
            sunsetTime = TimeSpan.FromHours(sunsetHour);
        }

        void Update()
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();
            UpdateTemperature();

            temp = Mathf.Lerp(currentTemperature, Furnace.Instance.totalHeatOutput, Furnace.Instance.temperatureInfluence);

            if (temperatureText != null)
                temperatureText.text = $"{temp:F1}°C - {GetCurrentSeason().name}";
        }

        private void UpdateTimeOfDay()
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

            if (timeText != null)
                timeText.text = currentTime.ToString("d/M/yyyy-HH:mm");
        }

        private void UpdateLightSettings()
        {
            float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
            sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
            moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
            RenderSettings.ambientLight = Color.Lerp(nightAmbientLight, dayAmbientLight, lightChangeCurve.Evaluate(dotProduct));
        }

        private void UpdateTemperature()
        {
            float yearPercent = GetYearProgress();
            float seasonBlend = yearlyTemperatureCurve.Evaluate(yearPercent);
            float globalMin = -45f;
            float globalMax = 27f;
            float seasonalAverage = Mathf.Lerp(globalMin, globalMax, seasonBlend);
            float dayFraction = (float)(currentTime.TimeOfDay.TotalHours / 24f);
            float dailyFactor = dailyTemperatureCurve.Evaluate(dayFraction);
            float dailyAmplitude = Mathf.Lerp(4f, 10f, seasonBlend);
            currentTemperature = seasonalAverage + (dailyFactor - 0.5f) * dailyAmplitude;
        }

        private void RotateSun()
        {
            float sunLightRotation;
            if (currentTime.TimeOfDay >= sunriseTime && currentTime.TimeOfDay <= sunsetTime)
            {
                TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
                TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);
                double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;
                sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
            }
            else
            {
                TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
                TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);
                double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
                sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
            }

            sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
        }

        private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
        {
            TimeSpan difference = toTime - fromTime;
            if (difference.TotalSeconds < 0)
                difference += TimeSpan.FromHours(24);
            return difference;
        }

        private SeasonTemperature GetCurrentSeason()
        {
            int month = currentTime.Month;
            if (month >= 6 && month <= 8) return seasons[0];
            if (month >= 9 && month <= 11) return seasons[1];
            if (month == 12 || month <= 2) return seasons[2];
            return seasons[3];
        }

        private float GetYearProgress()
        {
            if (DateTime.IsLeapYear(currentTime.Year))
                return (float)(currentTime.DayOfYear / 366f);
            return (float)(currentTime.DayOfYear / 365f);
        }
    }
}
