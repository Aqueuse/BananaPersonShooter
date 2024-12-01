using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class ManufacturedItemsInventory : MonoBehaviour {
        public Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        
        public void AddQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            if (manufacturedItemsInventory[manufacturedItemsType] > 10000) return;

            manufacturedItemsInventory[manufacturedItemsType] += quantity;

            var manufacturedItem = ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.uInventorySlots[manufacturedItemsType];
            
            manufacturedItem.gameObject.SetActive(true);
            manufacturedItem.SetQuantity(manufacturedItemsInventory[manufacturedItemsType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(manufacturedItem.itemScriptableObject, quantity);
        }
        
        public int RemoveQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            if (manufacturedItemsInventory[manufacturedItemsType] > quantity) {
                manufacturedItemsInventory[manufacturedItemsType] -= quantity;
            }

            else {
                manufacturedItemsInventory[manufacturedItemsType] = 0;
            }

            return manufacturedItemsInventory[manufacturedItemsType];
        }

        public int GetQuantity(ManufacturedItemsType manufacturedItemsType) {
            return manufacturedItemsInventory[manufacturedItemsType];
        }
    }
}