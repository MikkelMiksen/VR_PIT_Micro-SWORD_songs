using System.Collections.Generic;
using UnityEngine;

public class MJ_HandQuickSlots : MonoBehaviour
{
    [System.Serializable]
    public class HandSlotState
    {
        public Transform anchor;
        public List<ResourceType> slots = new List<ResourceType>();
        public int selectedIndex;
        public GameObject panelRoot;
        public List<GameObject> slotVisuals = new List<GameObject>();
    }

    [Header("References")]
    [SerializeField] private Transform leftHandAnchor;
    [SerializeField] private Transform rightHandAnchor;

    [Header("Layout")]
    [SerializeField] private Vector3 leftOffset = new Vector3(-0.18f, -0.10f, 0.10f);
    [SerializeField] private Vector3 rightOffset = new Vector3(0.18f, -0.10f, 0.10f);
    [SerializeField] private float slotSpacing = 0.12f;
    [SerializeField] private float slotSize = 0.06f;

    [Header("Defaults")]
    [SerializeField] private bool autoCreateOnStart = true;

    private HandSlotState leftHand = new HandSlotState();
    private HandSlotState rightHand = new HandSlotState();

    private void Awake()
    {
        if (leftHandAnchor == null)
        {
            leftHandAnchor = GameObject.Find("LeftHandAnchor")?.transform;
        }

        if (rightHandAnchor == null)
        {
            rightHandAnchor = GameObject.Find("RightHandAnchor")?.transform;
        }

        leftHand.anchor = leftHandAnchor;
        rightHand.anchor = rightHandAnchor;
    }

    private void Start()
    {
        if (autoCreateOnStart)
        {
            SetupDefaultSlots();
            RefreshAllVisuals();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleHand(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CycleHand(false);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            UseSelectedHandSlot(true);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            UseSelectedHandSlot(false);
        }
    }

    public void SetupDefaultSlots()
    {
        leftHand.slots = new List<ResourceType>
        {
            ResourceType.Sword,
            ResourceType.StoneAxe,
            ResourceType.CookedMeat,
            ResourceType.Berries
        };

        rightHand.slots = new List<ResourceType>
        {
            ResourceType.Sword,
            ResourceType.StoneAxe,
            ResourceType.CookedMeat,
            ResourceType.Berries
        };

        leftHand.selectedIndex = 0;
        rightHand.selectedIndex = 0;
    }

    public void SyncFromInventory()
    {
        if (MJ_PlayerInventory.Instance == null)
        {
            return;
        }

        var tools = MJ_PlayerInventory.Instance.GetToolsList();
        var foods = MJ_PlayerInventory.Instance.GetConsumablesList();

        leftHand.slots.Clear();
        rightHand.slots.Clear();

        foreach (var tool in tools)
        {
            if (leftHand.slots.Count < 4)
            {
                leftHand.slots.Add(tool.type);
            }
            else if (rightHand.slots.Count < 4)
            {
                rightHand.slots.Add(tool.type);
            }
        }

        foreach (var food in foods)
        {
            if (leftHand.slots.Count < 4)
            {
                leftHand.slots.Add(food.type);
            }
            else if (rightHand.slots.Count < 4)
            {
                rightHand.slots.Add(food.type);
            }
        }

        if (leftHand.slots.Count == 0)
        {
            leftHand.slots.Add(ResourceType.Sword);
        }

        if (rightHand.slots.Count == 0)
        {
            rightHand.slots.Add(ResourceType.Sword);
        }

        leftHand.selectedIndex = Mathf.Clamp(leftHand.selectedIndex, 0, leftHand.slots.Count - 1);
        rightHand.selectedIndex = Mathf.Clamp(rightHand.selectedIndex, 0, rightHand.slots.Count - 1);
        RefreshAllVisuals();
    }

    public void CycleHand(bool leftHandSide)
    {
        var hand = leftHandSide ? leftHand : rightHand;
        if (hand.slots.Count == 0)
        {
            return;
        }

        hand.selectedIndex = (hand.selectedIndex + 1) % hand.slots.Count;
        RefreshVisuals(hand);
    }

    public void UseSelectedHandSlot(bool leftHandSide)
    {
        var hand = leftHandSide ? leftHand : rightHand;
        if (hand.slots.Count == 0)
        {
            return;
        }

        ResourceType selectedType = hand.slots[hand.selectedIndex];

        if (MJ_PlayerInventory.Instance == null)
        {
            return;
        }

        if (MJ_PlayerInventory.Instance.IsFood(selectedType))
        {
            MJ_PlayerInventory.Instance.EatFood(selectedType);
            return;
        }

        MJ_PlayerInventory.Instance.TryEquipTool(selectedType);
    }

    private void RefreshAllVisuals()
    {
        RefreshVisuals(leftHand);
        RefreshVisuals(rightHand);
    }

    private void RefreshVisuals(HandSlotState hand)
    {
        if (hand.anchor == null)
        {
            return;
        }

        if (hand.panelRoot == null)
        {
            hand.panelRoot = new GameObject(hand.anchor.name + "_QuickSlots");
            hand.panelRoot.transform.SetParent(hand.anchor);
            hand.panelRoot.transform.localPosition = hand.anchor == leftHandAnchor ? leftOffset : rightOffset;
            hand.panelRoot.transform.localRotation = Quaternion.identity;
        }

        for (int i = hand.slotVisuals.Count; i < 4; i++)
        {
            GameObject slot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slot.name = hand.anchor.name + "_Slot_" + i;
            slot.transform.SetParent(hand.panelRoot.transform);
            slot.transform.localScale = new Vector3(slotSize, slotSize, 0.02f);
            slot.transform.localPosition = new Vector3((i - 1.5f) * slotSpacing, 0f, 0f);
            slot.transform.localRotation = Quaternion.identity;

            Renderer rend = slot.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material = new Material(Shader.Find("Standard"));
                rend.material.color = new Color(0.15f, 0.15f, 0.15f, 0.8f);
            }

            hand.slotVisuals.Add(slot);
        }

        for (int i = 0; i < hand.slotVisuals.Count; i++)
        {
            GameObject slot = hand.slotVisuals[i];
            if (slot == null)
            {
                continue;
            }

            Color color = GetColorForType(i < hand.slots.Count ? hand.slots[i] : ResourceType.Wood);
            if (i == hand.selectedIndex)
            {
                color = Color.Lerp(color, Color.white, 0.5f);
            }

            Renderer rend = slot.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = color;
            }

            slot.transform.localScale = i == hand.selectedIndex
                ? new Vector3(slotSize * 1.2f, slotSize * 1.2f, 0.04f)
                : new Vector3(slotSize, slotSize, 0.02f);
        }
    }

    private Color GetColorForType(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Sword:
            case ResourceType.Elucidator:
            case ResourceType.DarkRepulser:
            case ResourceType.LambentLight:
                return new Color(0.9f, 0.5f, 0.2f);
            case ResourceType.StoneAxe:
            case ResourceType.StonePickaxe:
                return new Color(0.7f, 0.7f, 0.7f);
            case ResourceType.CookedMeat:
            case ResourceType.Berries:
            case ResourceType.RawMeat:
                return new Color(0.3f, 0.8f, 0.3f);
            case ResourceType.Wood:
                return new Color(0.6f, 0.45f, 0.25f);
            case ResourceType.Coal:
                return new Color(0.1f, 0.1f, 0.1f);
            case ResourceType.Water:
                return new Color(0.2f, 0.5f, 0.9f);
            default:
                return new Color(0.5f, 0.5f, 0.5f);
        }
    }
}
