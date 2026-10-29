using UnityEngine;

namespace PIW_assets.player_inventory
{
    /// <summary>
    /// Inventory UI controller
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance;

        void Awake()
        {
            Instance = this;
        }

        public bool isShowing;

        public void Show()
        {
            isShowing = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            isShowing = false;
            gameObject.SetActive(false);
        }
    }
}
