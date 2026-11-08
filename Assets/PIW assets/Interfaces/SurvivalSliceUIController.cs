using System;
using System.Collections.Generic;
using PIW_assets.Resources;
using PIW_assets.Sword_interaction;
using PIW_assets.player_inventory;
using PIW_assets.worldBehaviour;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>Updates HUD and inventory UI Toolkit trees rendered by Unity 6.6 PanelRenderer.</summary>
public sealed class SurvivalSliceUIController : MonoBehaviour
{
    [SerializeField] private PanelRenderer hudPanelRenderer;
    [SerializeField] private PanelRenderer inventoryPanelRenderer;
    [SerializeField] private InputActionProperty toggleInventoryAction;
    [SerializeField, Min(0.05f)] private float refreshInterval = 0.15f;

    private VisualElement healthFill, hungerFill, temperatureFill, inventoryRoot, slotGrid;
    private Label healthLabel, hungerLabel, temperatureLabel, timeLabel, resultLabel;
    private Button closeButton;
    private float refreshTimer;
    private bool inventoryVisible;

    private void OnEnable()
    {
        if (hudPanelRenderer != null) hudPanelRenderer.RegisterUIReloadCallback(OnHudReload);
        if (inventoryPanelRenderer != null) inventoryPanelRenderer.RegisterUIReloadCallback(OnInventoryReload);
        if (toggleInventoryAction.action != null)
        {
            toggleInventoryAction.action.performed += OnToggleInventory;
            toggleInventoryAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (hudPanelRenderer != null) hudPanelRenderer.UnregisterUIReloadCallback(OnHudReload);
        if (inventoryPanelRenderer != null) inventoryPanelRenderer.UnregisterUIReloadCallback(OnInventoryReload);
        if (toggleInventoryAction.action != null)
        {
            toggleInventoryAction.action.performed -= OnToggleInventory;
            toggleInventoryAction.action.Disable();
        }
        if (closeButton != null) closeButton.clicked -= HideInventory;
    }

    private void OnHudReload(PanelRenderer renderer, VisualElement root, int version)
    {
        healthFill = root.Q<VisualElement>("HealthBarFill");
        hungerFill = root.Q<VisualElement>("HungerBarFill");
        temperatureFill = root.Q<VisualElement>("TempBarFill");
        healthLabel = root.Q<Label>("HealthLabel");
        hungerLabel = root.Q<Label>("HungerLabel");
        temperatureLabel = root.Q<Label>("TempLabel");
        timeLabel = root.Q<Label>("TimeLabel");
        resultLabel = root.Q<Label>("ResultLabel");
        Refresh();
    }

    private void OnInventoryReload(PanelRenderer renderer, VisualElement root, int version)
    {
        inventoryRoot = root.Q<VisualElement>("InventoryContainer");
        slotGrid = root.Q<VisualElement>("SlotGrid");
        closeButton = root.Q<Button>("CloseButton");
        if (closeButton != null)
        {
            closeButton.clicked -= HideInventory;
            closeButton.clicked += HideInventory;
        }
        HideInventory();
    }

    private void Update()
    {
        refreshTimer += Time.unscaledDeltaTime;
        if (refreshTimer < refreshInterval) return;
        refreshTimer = 0f;
        Refresh();
    }

    private void OnToggleInventory(InputAction.CallbackContext context)
    {
        if (inventoryVisible) HideInventory();
        else ShowInventory();
    }

    private void ShowInventory()
    {
        inventoryVisible = true;
        if (inventoryRoot != null) inventoryRoot.style.display = DisplayStyle.Flex;
        RefreshInventory();
    }

    private void HideInventory()
    {
        inventoryVisible = false;
        if (inventoryRoot != null) inventoryRoot.style.display = DisplayStyle.None;
    }

    private void Refresh()
    {
        PlayerStats stats = PlayerStats.Instance;
        MJ_PlayerHunger hunger = MJ_PlayerHunger.Instance;
        MJ_BodyTemp bodyTemp = MJ_BodyTemp.Instance;
        SetBar(healthFill, stats != null ? stats.currentHealth : 0f, stats != null ? stats.maxHealth : 1f);
        SetBar(hungerFill, hunger != null ? hunger.currentHunger : 0f, hunger != null ? hunger.maxHunger : 1f);
        SetBar(temperatureFill, bodyTemp != null ? bodyTemp.bodyTemp : 37f, 45f);

        if (healthLabel != null) healthLabel.text = stats != null ? $"HP {stats.currentHealth:0}/{stats.maxHealth:0}" : "HP --";
        if (hungerLabel != null) hungerLabel.text = hunger != null ? $"Hunger {hunger.currentHunger:0}%" : "Hunger --";
        if (temperatureLabel != null) temperatureLabel.text = bodyTemp != null ? $"{bodyTemp.bodyTemp:0.0}°C" : "Temp --";

        SurvivalSliceGameManager game = FindFirstObjectByType<SurvivalSliceGameManager>();
        if (timeLabel != null)
        {
            float remaining = game != null ? game.RemainingSeconds : 0f;
            timeLabel.text = game != null ? $"Survive: {TimeSpan.FromSeconds(remaining):mm\\:ss}" : "Survive: 02:00";
        }
        if (resultLabel != null && game != null && game.IsFinished)
            resultLabel.text = game.DidWin ? "You survived!" : "You were defeated";

        if (inventoryVisible) RefreshInventory();
    }

    private static void SetBar(VisualElement fill, float value, float maximum)
    {
        if (fill == null) return;
        float percent = maximum > 0f ? Mathf.Clamp01(value / maximum) * 100f : 0f;
        fill.style.width = Length.Percent(percent);
    }

    private void RefreshInventory()
    {
        if (slotGrid == null) return;
        slotGrid.Clear();
        MJ_PlayerInventory inventory = MJ_PlayerInventory.Instance;
        if (inventory == null)
        {
            slotGrid.Add(new Label("Inventory unavailable"));
            return;
        }

        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            int count = inventory.GetResourceAmount(type);
            if (count <= 0) continue;
            Label item = new Label($"{type}: {count}");
            item.AddToClassList("inventory-slot");
            slotGrid.Add(item);
        }
        if (slotGrid.childCount == 0) slotGrid.Add(new Label("Empty"));
    }
}
