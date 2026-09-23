using UnityEngine;

[CreateAssetMenu(fileName = "NewSwordData", menuName = "SAO/Sword Data")]
public class SwordData : ScriptableObject
{
    
    public string swordName;
    public ResourceType resourceType = ResourceType.Sword;
    public GameObject prefab;
    public float damage = 25f;
    public float staminaCost = 20f;
    
    // TODO: setup igame capture of sword models for sprite texture capture
    [Header("Visuals")]
    public Sprite icon;
    public Color themeColor = Color.cyan;
}
