using InGame.Items.ItemsProperties;
using UI.InGame.BottomSlotsPanel;
using UnityEngine;

namespace InGame.Player {
    public class BottomSlots : MonoBehaviour {
        [SerializeField] private Color activeSlotColor;
        [SerializeField] private Color unactiveSlotColor;
        
        public UIBottomSlot[] uiBottomSlots;
        
        public UIBottomSlot activeSlot;
        public int activeSlotIndex;
        
        public void Init() {
            RefreshSlotsQuantities();
        }
        
        public void SwitchToLeftSlot() {
            activeSlotIndex -= 1;
            if (activeSlotIndex < 0)
                activeSlotIndex = uiBottomSlots.Length - 1;
            
            ActivateSlot(activeSlotIndex);
        }
        
        public void SwitchToRightSlot() {
            activeSlotIndex += 1;
            if (activeSlotIndex > uiBottomSlots.Length - 1)
                activeSlotIndex = 0;

            ActivateSlot(activeSlotIndex);
        }

        public void ActivateSlot(int activeSlotIndex) {
            activeSlot = uiBottomSlots[this.activeSlotIndex];
            
            foreach (var uiBottomSlot in uiBottomSlots) {
                uiBottomSlot.SetSlotBackgroundColor(unactiveSlotColor);
            }

            uiBottomSlots[activeSlotIndex].SetSlotBackgroundColor(activeSlotColor);

            switch (activeSlot.slotType) {
                case SlotType.DROP:
                    ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.DROP);
                    break;
                case SlotType.BUILD:
                    ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.BUILD);
                    ObjectsReference.Instance.buildAction.SetActiveBuildable(activeSlot.itemScriptableObject.buildableType);
                    break;
                case SlotType.EMPTY:
                    ObjectsReference.Instance.bananaGunActionsSwitch.SwitchToBananaGunMode(BananaGunMode.IDLE);
                    return;
            }
        }
        
        // SetSelectedSlot
        public void SetSelectedSlot(ItemScriptableObject itemScriptableObject) {
            activeSlot.SetSlot(itemScriptableObject);
        }

        public ItemScriptableObject GetSelectedSlot() {
            return activeSlot.itemScriptableObject;
        }

        public void SetSlotByIndex(ItemScriptableObject itemScriptableObject, int index) {
            uiBottomSlots[index].SetSlot(itemScriptableObject);
        }
        
        public void RefreshSlotsQuantities() {
            foreach (var uiBottomSlot in uiBottomSlots) {
                uiBottomSlot.RefreshQuantity();
            }
        }
    }
}