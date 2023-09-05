using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIBananasInventory : MonoBehaviour {
        public GenericDictionary<BananaType, UInventorySlot> inventorySlotsByBananaType;
        
        private Dictionary<BananaType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.bananasInventory.bananasInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByBananaType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByBananaType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByBananaType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
    }
}
