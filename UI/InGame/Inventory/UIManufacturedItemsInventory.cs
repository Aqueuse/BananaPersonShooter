using InGame.Items.ItemsProperties.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIManufacturedItemsInventory : MonoBehaviour {
        public InventoryScriptableObject inventoryScriptableObject;
        public GenericDictionary<ManufacturedItemsType, UInventorySlot> uInventorySlots;
        
        [SerializeField] private Transform inventoryContentTransform; 
    
        [SerializeField] private CanvasGroup inventoryPanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        public void RefreshUInventory() {
            foreach (var inventoryItem in uInventorySlots) {
                if (inventoryScriptableObject.manufacturedItemsInventory[inventoryItem.Key] > 0) {
                    inventoryItem.Value.gameObject.SetActive(true);
                    inventoryItem.Value.GetComponent<UInventorySlot>()
                        .SetQuantity(inventoryScriptableObject.manufacturedItemsInventory[inventoryItem.Key]);
                }

                else inventoryItem.Value.gameObject.SetActive(false);
            }
        }
        
        public void Activate() {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedInventory = ItemCategory.BANANA;
        
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
    
        public void UnselectAllSlots() {
            foreach (var inventoryItem in uInventorySlots) {
                inventoryItem.Value.UnselectInventorySlot();
            }
        }

        public void SelectFirstSlot() {
            UnselectAllSlots();

            if (inventoryContentTransform.childCount == 0) return;

            foreach (var slot in inventoryContentTransform.GetComponentsInChildren<UInventorySlot>()) {
                if (slot.gameObject.activeInHierarchy && slot.itemScriptableObject != null) {
                    slot.GetComponent<UInventorySlot>().SelectInventorySlot();
                    break;
                }
            }
        }
    }
}