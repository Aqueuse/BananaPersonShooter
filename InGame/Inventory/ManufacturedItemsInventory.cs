using UnityEngine;

namespace InGame.Inventory {
    public class ManufacturedItemsInventory : MonoBehaviour {
        private GenericDictionary<ManufacturedItemsType, int> manufacturedItemsInventory;
        
        private void Start() {
            manufacturedItemsInventory = ObjectsReference.Instance.bananaMan.inventories.manufacturedItemsInventory;
            ObjectsReference.Instance.uiBananasInventory.inventoryScriptableObject = ObjectsReference.Instance.bananaMan.inventories;
        }
        
        public void AddQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            if (manufacturedItemsInventory[manufacturedItemsType] > 10000) return;

            manufacturedItemsInventory[manufacturedItemsType] += quantity;

            var manufacturedItem = ObjectsReference.Instance.uiManufacturedItemsItemsInventory.uInventorySlots[manufacturedItemsType];
            
            manufacturedItem.gameObject.SetActive(true);
            manufacturedItem.SetQuantity(manufacturedItemsInventory[manufacturedItemsType]);

            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(manufacturedItem.itemScriptableObject, quantity);
        }
        
        public void RemoveQuantity(ManufacturedItemsType manufacturedItemsType, int quantity) {
            var manufacturedItem = ObjectsReference.Instance.uiManufacturedItemsItemsInventory.uInventorySlots[manufacturedItemsType];

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