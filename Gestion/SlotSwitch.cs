using Enums;
using UI.InGame.QuickSlots;
using UnityEngine;

namespace Gestion {
    public class SlotSwitch : MonoBehaviour {
        public void SwitchSlot(UIQuickSlot slot) {
            if (slot.slotItemScriptableObject == null) {
                ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(BananaType.EMPTY, ItemCategory.EMPTY, BuildableType.EMPTY);
                ObjectsReference.Instance.build.CancelGhost();
                ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                return;
            }
            
            ObjectsReference.Instance.bananaMan.SetActiveItemTypeAndCategory(slot.slotItemScriptableObject.bananaType, slot.slotItemScriptableObject.itemCategory, slot.slotItemScriptableObject.buildableType);
            ObjectsReference.Instance.build.CancelGhost();

            switch (slot.slotItemScriptableObject.itemCategory) {
                case ItemCategory.BANANA:
                    ObjectsReference.Instance.bananaMan.activeItem =
                        ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ObjectsReference.Instance
                            .uiQuickSlotsManager.Get_Selected_Slot_Type());

                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(slot.slotItemScriptableObject.itemCategory, slot.slotItemScriptableObject.bananaType);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_banana_helper();

                    break;

                case ItemCategory.BUILDABLE:
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(slot.slotItemScriptableObject.itemCategory, BananaType.EMPTY);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                    break;

                case ItemCategory.EMPTY or ItemCategory.RAW_MATERIAL:
                    if (ObjectsReference.Instance.gameManager.isGamePlaying) ObjectsReference.Instance.bananaGun.UngrabBananaGun();
                    
                    ObjectsReference.Instance.uiCrosshairs.SetCrosshair(ItemCategory.EMPTY, BananaType.EMPTY);
                    ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().show_default_helper();
                    break;
            }
        }
    }
}