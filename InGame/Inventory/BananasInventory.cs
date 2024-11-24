using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class BananasInventory : MonoBehaviour {
        public Dictionary<BananaType, int> bananasInventory;

        private void Start() {
            bananasInventory = ObjectsReference.Instance.bananaMan.bananaManData.bananasInventory;
        }

        public void AddQuantity(BananaType bananaType, int quantity) {
            bananasInventory.TryAdd(bananaType, 0);
            
            if (bananasInventory[bananaType] > 10000) return;
            
            bananasInventory[bananaType] += quantity;

            var bananaData =
                ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType];
            
            ObjectsReference.Instance.uiFlippers.SetDroppableQuantity(ObjectsReference.Instance.inventoriesHelper.GetQuantity(bananaData));
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(bananaData, quantity);

            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                monkey.SearchForBananaManBananas();
            }
        }

        public int GetQuantity(BananaType bananaType) {
            return bananasInventory[bananaType];
        }

        public void RemoveQuantity(BananaType bananaType, int quantity) {
            if (bananasInventory[bananaType] > quantity) {
                bananasInventory[bananaType] -= quantity;
            }

            else {
                bananasInventory[bananaType] = 0;
            }
            
            ObjectsReference.Instance.uiFlippers.SetDroppableQuantity(bananasInventory[bananaType]);
        }
    }
}