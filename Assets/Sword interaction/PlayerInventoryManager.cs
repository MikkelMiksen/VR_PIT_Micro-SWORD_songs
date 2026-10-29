using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryManager : MonoBehaviour
{
    public static PlayerInventoryManager Instance;

    public Transform playerCamera;
    public EquipmentSlot leftShoulder;
    public EquipmentSlot rightShoulder;
    public List<GameObject> backpackSwords = new();

    void Awake() { Instance = this; }

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main != null ? Camera.main.transform : null;
            if (playerCamera == null)
            {
                var cam = FindAnyObjectByType<Camera>();
                if (cam != null) playerCamera = cam.transform;
            }
        }

        if (leftShoulder == null) leftShoulder = CreateSlot(EquipmentSlot.SlotType.ShoulderLeft);
        if (rightShoulder == null) rightShoulder = CreateSlot(EquipmentSlot.SlotType.ShoulderRight);
    }

    EquipmentSlot CreateSlot(EquipmentSlot.SlotType type)
    {
        GameObject go = new GameObject($"Slot_{type}");
        EquipmentSlot slot = go.AddComponent<EquipmentSlot>();
        slot.slotType = type;
        slot.playerCamera = playerCamera;
        return slot;
    }

    public void AddToBackpack(GameObject sword)
    {
        if (!backpackSwords.Contains(sword))
        {
            backpackSwords.Add(sword);
            sword.SetActive(false);
            sword.transform.SetParent(transform);
            Debug.Log($"[SAO] Added {sword.name} to backpack.");
        }
    }

    public void RemoveFromBackpack(GameObject sword)
    {
        if (backpackSwords.Contains(sword))
        {
            backpackSwords.Remove(sword);
        }
    }
}
