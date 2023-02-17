using Data;
using Enums;
using Settings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.BuildStation {
    public class UIBuildStationInventorySlotLeft : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI quantityText;
        [SerializeField] private TextMeshProUGUI descriptionText;

        public ItemThrowableType itemThrowableType;
        public ItemThrowableCategory itemThrowableCategory;
        public Toggle toggle;
    
        public void Select(string description) {
            descriptionText.text = ScriptableObjectManager.Instance.GetDescription(itemThrowableCategory, itemThrowableType, GameSettings.Instance.languageIndexSelected);
        }

        public void SetQuantity(int quantity) {
            quantityText.text = Game.Inventory.Instance.bananaManInventory[itemThrowableType].ToString();
        }
    }
}