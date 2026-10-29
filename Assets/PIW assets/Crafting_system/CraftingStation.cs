using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using PIW_assets.Crafting_system;

namespace PIW_assets.Crafting_system
{
    public class CraftingStation : MonoBehaviour, Iinteractable
    {
        public abstract WorkstationType StationType { get; }
        public abstract string StationName { get; }

        public virtual void Interact() { }
        public virtual string GetPrompt() => $"Use {StationName}";
    }

    public class CraftingUIController : MonoBehaviour
    {
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
            if (Instance == null) Instance = this;
            else { Destroy(gameObject); return; }

            if (_uiDocument == null) _uiDocument = GetComponent<UIDocument>();
            if (_uiDocument != null)
            {
                _root = _uiDocument.rootVisualElement;
                InitializeUI();
                HideUI();
            }
        }

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
        }

        public void HideUI()
        {
            if (_uiDocument != null) _uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            gameObject.SetActive(false);
        }

        private void OnRecipeSelected(string recipeName)
        {
            _selectedRecipe = _currentRecipes.FirstOrDefault(r => r.recipeName == recipeName);
            UpdateIngredientsDisplay();
        }

        private void UpdateIngredientsDisplay() { }
        private void OnCraftClicked() { }
    }
}
