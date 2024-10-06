using InGame.Items.ItemsProperties;
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

        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uInfobulle.SetDescriptionAndName(itemScriptableObject.GetName(), itemScriptableObject.GetDescription(), GetComponent<RectTransform>());
        }
        
        public void SelectInventorySlot() {
            if (itemScriptableObject == null) return;
            ObjectsReference.Instance.uInventoriesManager.SetLastSelectedItem(itemScriptableObject.itemCategory, gameObject);
        }

        public void UnselectInventorySlot() {
            GetComponent<Image>().color = Color.black;

            if (quantityText != null) quantityText.color = Color.yellow;
        }
    }
}
