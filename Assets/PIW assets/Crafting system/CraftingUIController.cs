using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
public class CraftingUIController : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static CraftingUIController Instance { get; private set; }

    [SerializeField] private UIDocument _uiDocument;
    
    private VisualElement _root;
    private DropdownField _recipeDropdown;
    private Label _ingredientsLabel;
    private Button _craftButton;
    private Button _closeButton;
    private Label _titleLabel;

    private List<RecipeSO> _currentRecipes;
    private RecipeSO _selectedRecipe;
    private CraftingStation _currentStation;
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_uiDocument == null)
            _uiDocument = GetComponent<UIDocument>();

        if (_uiDocument != null)
        {
            _root = _uiDocument.rootVisualElement;
            InitializeUI();
            HideUI();
        }
    }

    /// <summary>
    /// Initializes the UI elements
    /// </summary>
    private void InitializeUI()
    {
        _titleLabel = _root.Q<Label>("TitleLabel");
        _recipeDropdown = _root.Q<DropdownField>("RecipeDropdown");
        _ingredientsLabel = _root.Q<Label>("IngredientsLabel");
        _craftButton = _root.Q<Button>("CraftButton");
        _closeButton = _root.Q<Button>("CloseButton");

        _recipeDropdown.RegisterValueChangedCallback(evt => OnRecipeSelected(evt.newValue));
        _craftButton.clicked += OnCraftClicked;
        _closeButton.clicked += HideUI;
    }

    /// <summary>
    /// Opens the crafting UI for the given station and recipes
    /// </summary>
    /// <param name="station"></param>
    /// <param name="recipes"></param>
    public void ShowUI(CraftingStation station, List<RecipeSO> recipes)
    {
        _currentStation = station;
        _currentRecipes = recipes;

        if (_titleLabel != null)
            _titleLabel.text = station.StationName;

        _recipeDropdown.choices = recipes.Select(r => r.recipeName).ToList();
        
        if (recipes.Count > 0)
        {
            _recipeDropdown.value = recipes[0].recipeName;
            OnRecipeSelected(recipes[0].recipeName);
        }

        gameObject.SetActive(true);
        if (_uiDocument != null) _uiDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        
        Debug.Log($"[Crafting] Opened UI for {station.StationName}");
    }

    /// <summary>
    /// TODO: Add logic to close the crafting UI
    /// </summary>
    public void HideUI()
    {
        if (_uiDocument != null) _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// TODO: Add logic to check if the player has enough resources to craft the selected recipe
    /// </summary>
    /// <param name="recipeName"></param>
    private void OnRecipeSelected(string recipeName)
    {
        _selectedRecipe = _currentRecipes.FirstOrDefault(r => r.recipeName == recipeName);
        UpdateIngredientsDisplay();
    }

    /// <summary>
    /// TODO: Add logic to check if crafting is possible
    /// </summary>
    private void UpdateIngredientsDisplay()
    {
        if (_selectedRecipe == null)
        {
            _ingredientsLabel.text = "Select a recipe";
            _craftButton.SetEnabled(false);
            return;
        }

        string ingredientsText = "Requires:\n";
        bool canCraft = true;

        foreach (var ingredient in _selectedRecipe.ingredients)
        {
            int currentAmount = MJ_PlayerInventory.Instance.GetResourceAmount(ingredient.type);
            string color = currentAmount >= ingredient.amount ? "green" : "red";
            ingredientsText += $"- {ingredient.type}: <color={color}>{currentAmount}</color>/{ingredient.amount}\n";
            
            if (currentAmount < ingredient.amount)
                canCraft = false;
        }

        _ingredientsLabel.text = ingredientsText;
        _craftButton.SetEnabled(canCraft);
    }

    /// <summary>
    /// TODO: Add crafting logic
    /// </summary>
    private void OnCraftClicked()
    {
        if (_selectedRecipe == null) return;

        if (MJ_PlayerInventory.Instance.RemoveAllResources(_selectedRecipe.ingredients))
        {
            MJ_PlayerInventory.Instance.AddResource(_selectedRecipe.outputItem, _selectedRecipe.outputAmount, _selectedRecipe.toolDurability);
            Debug.Log($"[Crafting] Crafted {_selectedRecipe.outputAmount}x {_selectedRecipe.outputItem}");
            UpdateIngredientsDisplay();
        }
        else
        {
            Debug.LogWarning("[Crafting] Failed to remove resources even though button was enabled!");
        }
    }
}
