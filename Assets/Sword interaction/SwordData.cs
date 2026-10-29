using UnityEngine;

public class SwordData : ScriptableObject
{
    public string swordName;
    public ResourceType resourceType = ResourceType.Sword;
    public GameObject prefab;
    public float damage = 25f;
    public float staminaCost = 20f;
    public float knockbackForce = 4f;

    [Header("Visuals")]
    public Sprite icon;
    public Color themeColor = Color.cyan;
}
