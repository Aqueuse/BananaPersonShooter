using Enums;
using UnityEngine;

namespace UI.InGame.BuildStation {
    public class UIBuildStationInventoryLeft : MonoSingleton<UIBuildStationInventoryLeft> {
        [SerializeField] private GenericDictionary<ItemThrowableType, UIBuildStationInventorySlotLeft> buildInventorySlot;

        public void RefreshInventoySlots() {
            foreach (var item in global::Game.Inventory.Instance.bananaManInventory) {
                if (item.Key != ItemThrowableType.EMPTY) {
                    buildInventorySlot[item.Key].SetQuantity(global::Game.Inventory.Instance.bananaManInventory[item.Key]);
                
                    // if quantity is zero, don't show in the left panel
                    buildInventorySlot[item.Key].gameObject.SetActive(global::Game.Inventory.Instance.bananaManInventory[item.Key] != 0);
                }
            }
        }
    }
}