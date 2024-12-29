using InGame.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.MainPanel.Inventories {
    public class UIRawMaterialsInventory : MonoBehaviour {
        public RawMaterialInventory associatedRawMaterialInventory;
        
        public GenericDictionary<RawMaterialType, UInventorySlot> uInventorySlots;
        
        [SerializeField] private Transform inventoryContentTransform; 
    
        [SerializeField] private CanvasGroup inventoryPanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        public void RefreshUInventory() {
            foreach (var inventoryItem in uInventorySlots) {
                inventoryItem.Value.SetQuantity(associatedRawMaterialInventory.GetQuantity(inventoryItem.Value.itemScriptableObject));
            }
        }
        
        public void Activate() {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedInventory = DroppedType.RAW_MATERIAL;
        
            inventoryPanelCanvasGroup.alpha = 1;
            inventoryPanelCanvasGroup.interactable = true;
            inventoryPanelCanvasGroup.blocksRaycasts = true;
                
            buttonImage.color = activatedColor;
            buttonText.color = Color.black;
        }

        public void Desactivate() {
            inventoryPanelCanvasGroup.alpha = 0;
            inventoryPanelCanvasGroup.interactable = false;
            inventoryPanelCanvasGroup.blocksRaycasts = false;

            buttonImage.color = Color.black;
            buttonText.color = activatedColor;
        }
    
        private void UnselectAllSlots() {
            foreach (var inventoryItem in uInventorySlots) {
                inventoryItem.Value.UnselectInventorySlot();
            }
        }

        public void SelectFirstSlot() {
            UnselectAllSlots();

            if (inventoryContentTransform.childCount == 0) return;

            foreach (var slot in inventoryContentTransform.GetComponentsInChildren<UInventorySlot>()) {
                if (slot.gameObject.activeInHierarchy & slot.itemScriptableObject != null) {
                    slot.GetComponent<UInventorySlot>().SelectInventorySlot();
                    break;
                }
            }
        }
    }
}
