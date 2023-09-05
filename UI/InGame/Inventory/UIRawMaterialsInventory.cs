using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace UI.InGame.Inventory {
    public class UIRawMaterialsInventory : MonoBehaviour {
        public GenericDictionary<RawMaterialType, UInventorySlot> inventorySlotsByRawMaterialType;
        private Dictionary<BananaType, int> _itemsIndexByType;
        
        public void RefreshUInventory() {
            var inventory = ObjectsReference.Instance.rawMaterialsInventory.rawMaterialsInventory;

            foreach (var inventoryItem in inventory) {
                if (inventoryItem.Value > 0) {
                    inventorySlotsByRawMaterialType[inventoryItem.Key].gameObject.SetActive(true);
                    inventorySlotsByRawMaterialType[inventoryItem.Key].GetComponent<UInventorySlot>().SetQuantity(inventoryItem.Value);
                }

                else inventorySlotsByRawMaterialType[inventoryItem.Key].gameObject.SetActive(false);
            }
        }
    }
}
