using UnityEngine;

public class MJ_PlayerHunger : MonoBehaviour
{
    public static MJ_PlayerHunger Instance { get; private set; }

    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDepletionRate = 0.5f; // per second
    public float starvationDamage = 2f; // per second when hunger is 0

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        currentHunger = maxHunger;
    }

    private void Update()
    {
        DepleteHunger();
        CheckStarvation();
    }

    private void DepleteHunger()
    {
        currentHunger -= hungerDepletionRate * Time.deltaTime;
        currentHunger = Mathf.Max(currentHunger, 0);
    }

    private void CheckStarvation()
    {
        if (currentHunger <= 0)
        {
            if (PlayerStats.Instance != null)
            {
                PlayerStats.Instance.TakeDamage(starvationDamage * Time.deltaTime);
            }
        }
    }

    public void Eat(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Min(currentHunger, maxHunger);
        Debug.Log($"[Hunger] Ate food, current hunger: {currentHunger:F1}");
    }
}
