using InGame.Items.ItemsProperties;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UInventorySlot : MonoBehaviour {
        public ItemScriptableObject itemScriptableObject;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        public void Activate() {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GESTION_VIEW) return;
            
            ObjectsReference.Instance.bottomSlots.SetSelectedSlot(itemScriptableObject);
        }
        
        public void SetDescriptionAndName() {
            ObjectsReference.Instance.uiToolTipOnMouseHover.SetSlotInfo(itemScriptableObject, GetComponent<RectTransform>());
        }
        
        public void SelectInventorySlot() {
            if (itemScriptableObject == null) return;
            ObjectsReference.Instance.uInventoriesManager.SetLastSelectedItem(itemScriptableObject.droppedType, gameObject);
        }

        public void UnselectInventorySlot() {
            GetComponent<Image>().color = Color.black;

            if (quantityText != null) quantityText.color = Color.yellow;
        }
        
        public string GetQuantity() {
            return quantityText.text;
        }
        
        public void SetQuantity(int quantity) {
            if (quantityText != null) quantityText.text = quantity > 999 ? "999+" : quantity.ToString();
        }
    }
}
