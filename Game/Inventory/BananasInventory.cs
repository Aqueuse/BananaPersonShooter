using Data.Bananas;
using Gestion;
using UnityEngine;

namespace Game.Inventory {
    public class BananasInventory : MonoBehaviour {
        public GenericDictionary<BananaType, int> bananasInventory;
        
        public void AddQuantity(BananasDataScriptableObject bananasDataScriptableObject, int quantity) {
            if (bananasInventory[bananasDataScriptableObject.bananaType] > 10000) return;
            
            bananasInventory[bananasDataScriptableObject.bananaType] += quantity;
            
            var bananaItem = ObjectsReference.Instance.uiBananasInventory.inventorySlotsByBananaType[bananasDataScriptableObject.bananaType];
            bananaItem.gameObject.SetActive(true);
            bananaItem.SetQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);

            if (ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject == bananasDataScriptableObject)
                ObjectsReference.Instance.quickSlotsManager.SetBananaQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(bananasDataScriptableObject, quantity);

            foreach (var monkey in MapItems.Instance.monkeys) {
                monkey.SearchForBananaManBananas();
            }
        }

        public int GetQuantity(BananaType bananaType) {
            return bananasInventory[bananaType];
        }

        public void RemoveQuantity(BananaType bananaType, int quantity) {
            var bananaItem = ObjectsReference.Instance.uiBananasInventory.inventorySlotsByBananaType[bananaType];

            if (bananasInventory[bananaType] > quantity) {
                bananasInventory[bananaType] -= quantity;
                bananaItem.SetQuantity(bananasInventory[bananaType]);
            }

            else {
                bananasInventory[bananaType] = 0;
                bananaItem.SetQuantity(0);
                bananaItem.gameObject.SetActive(false);
            }
            
            if (ObjectsReference.Instance.quickSlotsManager.bananaSlotItemScriptableObject.bananaType == bananaType)
                ObjectsReference.Instance.quickSlotsManager.SetBananaQuantity(bananasInventory[bananaType]);
        }

        public bool HasBananas() {
            foreach (var bananas in bananasInventory) {
                if (bananas.Value > 0) return true;
            }

            return false;
        }
    }
}