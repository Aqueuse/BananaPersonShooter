using Data;
using Enums;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.PlateformBuilder {
    public class UIBuildInventorySlotLeft : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public ItemThrowableType itemThrowableType;
        public Toggle toggle;
    
        public void Select(string description) {
            descriptionText.text = ScriptableObjectManager.Instance.GetDescription(itemThrowableType, GameSettings.Instance.languageIndexSelected);
        }

        public void SetQuantity(int quantity) {
            quantityText.text = global::Inventory.Instance.bananaManInventory[itemThrowableType].ToString();
        }
    }
}
