using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace PIW_assets.Crafting_system
{
    public class CraftingUIController : MonoBehaviour
    {
        public static CraftingUIController Instance { get; private set; }

        [SerializeField] private PanelRenderer panelRenderer;

        private VisualElement _root;
        private VisualElement _craftingPanel;
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
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }
            if (panelRenderer == null) panelRenderer = GetComponent<PanelRenderer>();
        }

        private void OnEnable()
        {
            if (panelRenderer != null) panelRenderer.RegisterUIReloadCallback(OnUIReload);
        }

        private void OnDisable()
        {
            if (panelRenderer != null) panelRenderer.UnregisterUIReloadCallback(OnUIReload);
            if (_craftButton != null) _craftButton.clicked -= OnCraftClicked;
            if (_closeButton != null) _closeButton.clicked -= HideUI;
        }

        private void OnUIReload(PanelRenderer renderer, VisualElement root, int version)
        {
            _root = root;
            _craftingPanel = root.Q<VisualElement>("CraftingPanel");
            _titleLabel = root.Q<Label>("TitleLabel");
            _recipeDropdown = root.Q<DropdownField>("RecipeDropdown");
            _ingredientsLabel = root.Q<Label>("IngredientsLabel");
            _craftButton = root.Q<Button>("CraftButton");
            _closeButton = root.Q<Button>("CloseButton");

            if (_recipeDropdown != null)
                _recipeDropdown.RegisterValueChangedCallback(evt => OnRecipeSelected(evt.newValue));
            if (_craftButton != null) _craftButton.clicked += OnCraftClicked;
            if (_closeButton != null) _closeButton.clicked += HideUI;
            HideUI();
        }

        public void ShowUI(CraftingStation station, List<RecipeSO> recipes)
        {
            if (_craftingPanel == null || station == null || recipes == null) return;
            _currentStation = station;
            _currentRecipes = recipes;

            if (_titleLabel != null) _titleLabel.text = station.StationName;
            if (_recipeDropdown != null) _recipeDropdown.choices = recipes.Select(r => r.recipeName).ToList();

            if (recipes.Count > 0 && _recipeDropdown != null)
            {
                _recipeDropdown.value = recipes[0].recipeName;
                OnRecipeSelected(recipes[0].recipeName);
            }
            _craftingPanel.style.display = DisplayStyle.Flex;
        }

        public void HideUI()
        {
            if (_craftingPanel != null) _craftingPanel.style.display = DisplayStyle.None;
        }

        private void OnRecipeSelected(string recipeName)
        {
            if (_currentRecipes == null) return;
            _selectedRecipe = _currentRecipes.FirstOrDefault(r => r.recipeName == recipeName);
            UpdateIngredientsDisplay();
        }

        private void UpdateIngredientsDisplay() { }
        private void OnCraftClicked() { }
    }
}
