using InGame.Items.ItemsProperties;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.BottomSlotsPanel {
    public class UIBottomSlot : MonoBehaviour {
        public ItemScriptableObject itemScriptableObject;
        [SerializeField] private Image slotImage;
        
        [SerializeField] private Image slotItemImage;
        [SerializeField] private TextMeshProUGUI slotItemQuantityText;

        public SlotType slotType;
        public int slotIndex;

        // SetSelectedSlot
        public void SetSlot(ItemScriptableObject itemScriptableObject) {
            this.itemScriptableObject = itemScriptableObject;
            slotItemImage.sprite = this.itemScriptableObject.GetSprite();
            slotItemImage.enabled = true;

            if (this.itemScriptableObject.itemCategory == ItemCategory.BUILDABLE) {
                slotType = SlotType.BUILD;

                ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.BUILD);
                ObjectsReference.Instance.buildAction.SetActiveBuildable(this.itemScriptableObject.buildableType);
                
                slotItemQuantityText.text = "";
            }
            
            else {
                RefreshQuantity();
                slotType = SlotType.DROP;
                ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.DROP);
            }
        }

        public void RefreshQuantity() {
            if (itemScriptableObject == null) return;
            if (itemScriptableObject.itemCategory == ItemCategory.BUILDABLE) return;
            
            slotItemQuantityText.text = ObjectsReference.Instance.bananaMan.bananaManData.inventoriesByDroppedType[itemScriptableObject.droppedType].GetQuantity(itemScriptableObject).ToString();
        }

        public void SetSlotBackgroundColor(Color color) {
            slotImage.color = color;
        }

        public void ClearSlot() {
            ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.IDLE);
            
            ResetSlot();
        }

        public void ResetSlot() {
            slotItemImage.sprite = null;
            slotItemImage.enabled = false;
            slotItemQuantityText.text = "";

            slotType = SlotType.EMPTY;
        }

        public string GenerateSavedData() {
            var slotData = "";

            slotData += itemScriptableObject.itemCategory + "-";
            slotData += itemScriptableObject.buildableType + "-";
            slotData += itemScriptableObject.bananaType + "-";
            slotData += itemScriptableObject.rawMaterialType + "-";
            slotData += itemScriptableObject.ingredientsType + "-";
            slotData += itemScriptableObject.foodType + "-";
            slotData += itemScriptableObject.manufacturedItemsType + "-";
            slotData += slotIndex;
            
            return slotData;
        }
    }
}