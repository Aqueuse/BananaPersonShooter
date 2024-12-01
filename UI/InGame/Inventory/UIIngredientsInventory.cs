using InGame.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Inventory {
    public class UIIngredientsInventory : MonoBehaviour {
        public IngredientsInventory associatedIngredientsInventory;
        public GenericDictionary<IngredientsType, UInventorySlot> uInventorySlots;
        
        [SerializeField] private Transform inventoryContentTransform; 
        
        [SerializeField] private CanvasGroup inventoryPanelCanvasGroup;
        [SerializeField] private Image buttonImage;
        [SerializeField] private TextMeshProUGUI buttonText;

        [SerializeField] private Color activatedColor;

        public void RefreshUInventory() {
            foreach (var inventoryItem in uInventorySlots) {
                inventoryItem.Value.GetComponent<UInventorySlot>()
                    .SetQuantity(associatedIngredientsInventory.ingredientsInventory[inventoryItem.Key]);
            }
        }
        
        public void Activate() {
            ObjectsReference.Instance.uInventoriesManager.lastFocusedInventory = DroppedType.INGREDIENTS;
        
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
        
        public void SelectFirstSlot() {
            inventoryContentTransform.GetComponentsInChildren<UInventorySlot>()[0].GetComponent<UInventorySlot>().SelectInventorySlot();
        }
    }
}
