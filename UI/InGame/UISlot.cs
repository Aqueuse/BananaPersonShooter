using Enums;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame {
    public class UISlot : MonoBehaviour {
        [SerializeField] private RectTransform favoriteBackground;
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI quantityText;
        
        private BananaType _bananaType = BananaType.EMPTY_HAND;
        
        public void SetSelectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(125f, 125f);

            BananaMan.Instance.activeBananaType = _bananaType;
            BananaMan.Instance.isArmed = BananaMan.Instance.activeBananaType != BananaType.EMPTY_HAND;
        }

        public void SetUnselectedWeaponSlot() {
            favoriteBackground.sizeDelta = new Vector2(100f, 100f);
        }

        public void SetSlot(BananaType bananaType) {
            _bananaType = bananaType;
            
            iconImage.sprite = UInventory.Instance.lastselectedInventoryItem.transform.GetChild(0).GetComponentInChildren<Image>().sprite;
            if (bananaType == BananaType.EMPTY_HAND) {
                SetAmmoQuantity("");
            }

            else {
                SetAmmoQuantity(Inventory.Instance.GetQuantity(bananaType).ToString());
            }
        }
        
        public void SetAmmoQuantity(string quantity) {
            quantityText.text = quantity;
        }
    }
}
