using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PIW_assets.Resources;

namespace PIW_assets.Furnace
{
    public class FurnaceUI : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Dropdown inputTypeDropdown;
        public TMP_InputField inputAmountField;
        public Image resourceImage;
        public TextMeshProUGUI woodText, coalText, dieselText;
        public Button confirmButton;

        [Header("Resource Sprites")]
        public Sprite woodSprite;
        public Sprite stoneSprite;
        public Sprite metalSprite;
        public Sprite coalSprite;
        public Sprite waterSprite;
        public Sprite dieselSprite;

        private ResourceType selectedResource;
        private int enteredAmount;

        private void Start()
        {
            inputTypeDropdown.ClearOptions();
            inputTypeDropdown.AddOptions(System.Enum.GetNames(typeof(ResourceType)).ToList());

            inputTypeDropdown.onValueChanged.AddListener(OnResourceChanged);
            inputAmountField.onValueChanged.AddListener(OnAmountChanged);
            confirmButton.onClick.AddListener(OnConfirmClicked);

            OnResourceChanged(0);
            Hide();
        }

        private void OnResourceChanged(int index)
        {
            selectedResource = (ResourceType)index;

            switch (selectedResource)
            {
                case ResourceType.Wood: resourceImage.sprite = woodSprite; break;
                case ResourceType.Stone: resourceImage.sprite = stoneSprite; break;
                case ResourceType.Metal: resourceImage.sprite = metalSprite; break;
                case ResourceType.Coal: resourceImage.sprite = coalSprite; break;
                case ResourceType.Water: resourceImage.sprite = waterSprite; break;
                case ResourceType.Diesel: resourceImage.sprite = dieselSprite; break;
            }
        }

        private void OnAmountChanged(string input)
        {
            if (int.TryParse(input, out int value))
                enteredAmount = Mathf.Max(0, value);
            else
                enteredAmount = 0;
        }

        private void OnConfirmClicked()
        {
            if (enteredAmount <= 0)
            {
                Debug.Log("❌ Enter a valid amount!");
                return;
            }

            Furnace.Instance.AddFuel(selectedResource, enteredAmount);
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        void Update()
        {
            woodText.text = Furnace.Instance.GetResourceAmount(ResourceType.Wood);
            coalText.text = Furnace.Instance.GetResourceAmount(ResourceType.Coal);
            dieselText.text = Furnace.Instance.GetResourceAmount(ResourceType.Diesel);
        }
    }
}
