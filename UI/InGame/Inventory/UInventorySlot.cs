using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UInventorySlot : MonoBehaviour {
        public ItemScriptableObject itemScriptableObject;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        public string GetQuantity() {
            return quantityText.text;
        }
        
        public void SetQuantity(int quantity) {
            if (quantityText != null) quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }

        private void SetDescriptionAndName() {
            ObjectsReference.Instance.descriptionsManager.ShowPanel(itemScriptableObject.gameObjectTag);
            ObjectsReference.Instance.descriptionsManager.SetDescription(itemScriptableObject);
            ObjectsReference.Instance.descriptionsManager.inventoryGestionPanel.SetDescription(itemScriptableObject);
        }

        public void SelectInventorySlot() {
            ObjectsReference.Instance.uInventoriesManager.UnselectInventorySlots(itemScriptableObject.itemCategory);

            GetComponent<Image>().color = Color.yellow;
            if (quantityText != null) quantityText.color = Color.black;
            
            SetDescriptionAndName();
            
            ObjectsReference.Instance.uInventoriesManager.SetLastSelectedItem(itemScriptableObject.itemCategory, gameObject);
        }

        public void UnselectInventorySlot() {
            GetComponent<Image>().color = Color.black;
            
            if (quantityText != null) quantityText.color = Color.yellow;
        }
    }
}
