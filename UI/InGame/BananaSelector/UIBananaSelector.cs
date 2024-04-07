using InGame.Items.ItemsProperties.Characters;
using UI.InGame.Inventory;
using UnityEngine;

namespace UI.InGame.BananaSelector {
    public class UIBananaSelector : MonoBehaviour {
        public InventoryScriptableObject inventoryScriptableObject;
        public GenericDictionary<BananaType, UIBananaSlot> uiBananaSelectorSlots;
    
        public void RefreshBananaSelector() {
            foreach (var inventoryItem in uiBananaSelectorSlots) {
                if (inventoryScriptableObject.bananasInventory[inventoryItem.Key] > 0) {
                    inventoryItem.Value.gameObject.SetActive(true);
                    inventoryItem.Value.GetComponent<UInventorySlot>()
                        .SetQuantity(inventoryScriptableObject.bananasInventory[inventoryItem.Key]);
                }

                else inventoryItem.Value.gameObject.SetActive(false);
            }
        }
    }
}
