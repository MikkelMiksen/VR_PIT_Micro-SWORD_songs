using UnityEngine;

/// <summary>
/// Body temperature controller
/// </summary>
public class MJ_BodyTemp : MonoBehaviour
{
    public static MJ_BodyTemp Instance; void Awake() { Instance = this; }

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

    /// <summary>
    /// Update the body temperature
    /// </summary>
    void Update()
    {
        UpdateBodyTemperature();
        CheckTemperatureEffects();
        UpdateCurrentAreaTemp();
        ApplyTemperatureDamage();
    }

    /// <summary>
    /// Apply damage to the player if body temperature is dangerous
    /// </summary>
    private void ApplyTemperatureDamage()
    {
        if (PlayerStats.Instance != null && !isBodyTempSafe)
        {
            // Damage player if body temp is dangerous
            // Using a small amount per frame/second
            float damage = 2f * Time.deltaTime; 
            PlayerStats.Instance.TakeDamage(damage);
        }
    }

    /// <summary>
    /// Update the current area temperature
    /// </summary>
    private void UpdateCurrentAreaTemp()
    {
        if (DayNightCycle.Instance != null)
        {
            currentAreaTemp = DayNightCycle.Instance.temp;
        }
    }

    /// <summary>
    /// Adjust the body temperature based on the current area temperature
    /// </summary>
    private void UpdateBodyTemperature()
    {
        float environmentTemp = currentAreaTemp;

        // difference between body and environment
        float delta = environmentTemp - bodyTemp;

        // scale adjustment speed based on magnitude of difference
        // this makes big gaps faster, small gaps slower
        float adjustment = Mathf.Sign(delta) * Mathf.Pow(Mathf.Abs(delta), 1.3f);


        // smooth approach with diminishing speed near extremes
        bodyTemp += adjustment * temperatureChangeRate * Time.deltaTime;

        // clamp to safe physiological limits
        bodyTemp = Mathf.Clamp(bodyTemp, 19.8f, 45f);
    }

    /// <summary>
    /// Check the body temperature and set the appropriate flags
    /// </summary>
    private void CheckTemperatureEffects()
    {
        if (bodyTemp >= minSafeBodyTemp && bodyTemp <= maxSafeBodyTemp)
        { // Temperature is safe
            readBodyTemp = $"Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = true;
            isBodyTempFreezing = false;
            isBodyTempOverheating = false;
            hpInfluenceCoefficient = 1;

        }
        else if (bodyTemp <= minSafeBodyTemp)
        { // Temperature is cold
            readBodyTemp = $"Hypothermia risk! Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = false;
            isBodyTempFreezing = true;
            isBodyTempOverheating = false;
            hpInfluenceCoefficient = 0.96f;
        }
        else if (bodyTemp >= maxSafeBodyTemp)
        { //Temperature is high
            readBodyTemp = $"Overheating risk! Body temp: {bodyTemp:F1}°C";
            isBodyTempSafe = false;
            isBodyTempFreezing = false;
            isBodyTempOverheating = true;
            hpInfluenceCoefficient = 0.96f;
        }
    }
}
