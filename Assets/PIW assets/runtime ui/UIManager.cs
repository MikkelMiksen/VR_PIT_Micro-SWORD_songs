using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Documents")]
    [SerializeField] private UIDocument hudDocument;
    [SerializeField] private UIDocument inventoryDocument;
    [SerializeField] private UIDocument craftingDocument;

    [Header("UI State")]
    public bool isInventoryOpen = false;
    public bool isCraftingOpen = false;

    private VisualElement hudRoot;
    private VisualElement inventoryRoot;
    private VisualElement craftingRoot;

    private VisualElement healthBar;
    private VisualElement tempBar;
    private Label tempLabel;
    private Label timeLabel;
    private VisualElement inventorySlotGrid;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        InitializeHUD();
        InitializeInventory();
        InitializeCrafting();
        
        SetInventoryActive(false);
        SetCraftingActive(false);
    }

    private void InitializeHUD()
    {
        if (hudDocument == null) return;
        hudRoot = hudDocument.rootVisualElement;
        healthBar = hudRoot.Q<VisualElement>("HealthBarFill");
        tempBar = hudRoot.Q<VisualElement>("TempBarFill");
        tempLabel = hudRoot.Q<Label>("TempLabel");
        timeLabel = hudRoot.Q<Label>("TimeLabel");
    }

    private void InitializeInventory()
    {
        if (inventoryDocument == null) return;
        inventoryRoot = inventoryDocument.rootVisualElement;
        inventorySlotGrid = inventoryRoot.Q<VisualElement>("SlotGrid");
        var closeBtn = inventoryRoot.Q<Button>("CloseButton");
        if (closeBtn != null) closeBtn.clicked += () => SetInventoryActive(false);
    }

    private void InitializeCrafting()
    {
        if (craftingDocument == null) return;
        craftingRoot = craftingDocument.rootVisualElement;
        var closeBtn = craftingRoot.Q<Button>("CloseButton");
        if (closeBtn != null) closeBtn.clicked += () => SetCraftingActive(false);
    }

    void Update()
    {
        UpdateHUD();
        
        // Toggle Inventory (for testing, ideally triggered by VR controller)
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetInventoryActive(!isInventoryOpen);
        }
    }

    private void UpdateHUD()
    {
        if (PlayerStats.Instance != null)
        {
            float hpPercent = PlayerStats.Instance.currentHealth / PlayerStats.Instance.maxHealth;
            if (healthBar != null) healthBar.style.width = Length.Percent(hpPercent * 100f);
        }

        if (MJ_BodyTemp.Instance != null)
        {
            if (tempLabel != null) tempLabel.text = $"{MJ_BodyTemp.Instance.bodyTemp:F1}°C";
            // Map body temp to bar fill (e.g., 20 to 45 degree range)
            float t = Mathf.InverseLerp(20f, 45f, MJ_BodyTemp.Instance.bodyTemp);
            if (tempBar != null) tempBar.style.width = Length.Percent(t * 100f);
        }

        if (DayNightCycle.Instance != null && timeLabel != null)
        {
            timeLabel.text = DayNightCycle.Instance.currentTime.ToString("HH:mm");
        }
    }

    public void SetInventoryActive(bool active)
    {
        isInventoryOpen = active;
        if (inventoryDocument != null) inventoryDocument.rootVisualElement.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        
        if (active)
        {
            SetCraftingActive(false);
            RefreshInventoryUI();
        }
    }

    public void SetCraftingActive(bool active)
    {
        isCraftingOpen = active;
        if (craftingDocument != null) craftingDocument.rootVisualElement.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        
        if (active) SetInventoryActive(false);
    }

    public void RefreshInventoryUI()
    {
        if (inventorySlotGrid == null || MJ_PlayerInventory.Instance == null) return;
        
        inventorySlotGrid.Clear();
        
        foreach (var slot in MJ_PlayerInventory.Instance.Resources)
        {
            if (slot.IsEmpty) continue;

            var slotElement = new VisualElement();
            slotElement.AddToClassList("inventory-slot");

            var icon = new VisualElement();
            icon.AddToClassList("slot-icon");
            var sprite = MJ_PlayerInventory.Instance.GetSpriteForType(slot.type);
            if (sprite != null)
            {
                icon.style.backgroundImage = new StyleBackground(sprite.texture);
            }
            slotElement.Add(icon);

            var countLabel = new Label(slot.amount.ToString());
            countLabel.AddToClassList("slot-count");
            slotElement.Add(countLabel);

            inventorySlotGrid.Add(slotElement);
        }
    }
}
