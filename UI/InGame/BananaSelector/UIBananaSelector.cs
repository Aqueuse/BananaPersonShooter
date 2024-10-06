using InGame.Inventory;
using UI.InGame.Inventory;
using UnityEngine;

namespace UI.InGame.BananaSelector {
    public class UIBananaSelector : MonoBehaviour {
        public BananasInventory bananasInventory;
        public GenericDictionary<BananaType, UIBananaSlot> uiBananaSelectorSlots;

        private void Start() {
            bananasInventory = ObjectsReference.Instance.bananasInventory;
        }

        public void RefreshBananaSelector() {
            foreach (var inventoryItem in uiBananaSelectorSlots) {
                if (bananasInventory.bananasInventory[inventoryItem.Key] > 0) {
                    inventoryItem.Value.gameObject.SetActive(true);
                    inventoryItem.Value.GetComponent<UInventorySlot>()
                        .SetQuantity(bananasInventory.bananasInventory[inventoryItem.Key]);
                }

                else inventoryItem.Value.gameObject.SetActive(false);
            }
        }
    }
}
