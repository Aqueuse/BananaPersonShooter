using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class ManufacturedItemsInventory : MonoBehaviour {
        private Dictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        
        private void Start() {
            manufacturedItemsInventory = ObjectsReference.Instance.bananaMan.bananaManData.manufacturedItemsInventory;
        }
        
        public void AddQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            if (manufacturedItemsInventory[manufacturedItemsType] > 10000) return;

            manufacturedItemsInventory[manufacturedItemsType] += quantity;

            var manufacturedItem = ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.uInventorySlots[manufacturedItemsType];
            
            manufacturedItem.gameObject.SetActive(true);
            manufacturedItem.SetQuantity(manufacturedItemsInventory[manufacturedItemsType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(manufacturedItem.itemScriptableObject, quantity);
        }
        
        public void RemoveQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            var manufacturedItem = ObjectsReference.Instance.bananaManUiManufacturedItemsInventory.uInventorySlots[manufacturedItemsType];

            if (manufacturedItemsInventory[manufacturedItemsType] > quantity) {
                manufacturedItemsInventory[manufacturedItemsType] -= quantity;
                manufacturedItem.SetQuantity(manufacturedItemsInventory[manufacturedItemsType]);
            }

            else {
                manufacturedItemsInventory[manufacturedItemsType] = 0;
                manufacturedItem.SetQuantity(0);
                manufacturedItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(manufacturedItem.itemScriptableObject, quantity);
        }
    }
}