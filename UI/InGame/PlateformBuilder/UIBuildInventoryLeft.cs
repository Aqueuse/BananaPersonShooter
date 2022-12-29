using Enums;
using UnityEngine;

namespace UI.InGame.PlateformBuilder {
    public class UIBuildInventoryLeft : MonoSingleton<UIBuildInventoryLeft> {
        [SerializeField] private GenericDictionary<ItemThrowableType, UIBuildInventorySlotLeft> buildInventorySlot;

        public void RefreshInventoySlots() {
            foreach (var item in global::Inventory.Instance.bananaManInventory) {
                buildInventorySlot[item.Key].SetQuantity(global::Inventory.Instance.bananaManInventory[item.Key]);
                
                // if quantity is zero, don't show in the left panel
                buildInventorySlot[item.Key].gameObject.SetActive(global::Inventory.Instance.bananaManInventory[item.Key] != 0);
            }
        }
    }
}