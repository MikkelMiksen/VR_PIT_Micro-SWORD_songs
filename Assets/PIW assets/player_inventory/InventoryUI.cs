using System.Collections.Generic;
using PIW_assets.Crafting_system;
using PIW_assets.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PIW_assets.player_inventory
{
    public class InventoryUI : MonoBehaviour
    {
        public static InventoryUI Instance;
        public bool isShowing;

        void Awake() => Instance = this;

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
