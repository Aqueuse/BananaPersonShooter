using System;
using Enums;
using Player;
using TMPro;
using UI.InGame.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UISlot : MonoBehaviour {
        [SerializeField] private RectTransform favoriteBackground;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;
    
        private ItemThrowableType _itemThrowableType;
        private Color transparent;

        private void Start() {
            transparent = Color.white;
        }

        public void SetSelectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(125f, 125f);

            BananaMan.Instance.activeItemThrowableType = _itemThrowableType;
        }

        public void SetUnselectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(100f, 100f);
        }

        public void SetSlot(ItemThrowableType itemThrowableType) {
            _itemThrowableType = itemThrowableType;
        
            iconImage.sprite = UInventory.Instance.lastselectedInventoryItem.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            SetAmmoQuantity(global::Inventory.Instance.GetQuantity(itemThrowableType).ToString());
            iconImage.color = transparent;
        }
    
        public void SetAmmoQuantity(string quantity) {
            quantityText.text = quantity;
        }
    }
}
