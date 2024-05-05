using InGame.Items.ItemsProperties.Bananas;
using UnityEngine;

namespace InGame.Inventory {
    public class BananasInventory : MonoBehaviour {
        private GenericDictionary<BananaType, int> bananasInventory;

        private void Start() {
            bananasInventory = ObjectsReference.Instance.bananaMan.inventories.bananasInventory;
        }

        public void AddQuantity(BananasPropertiesScriptableObject bananasDataScriptableObject, int quantity) {
            if (bananasInventory[bananasDataScriptableObject.bananaType] > 10000) return;
            
            bananasInventory[bananasDataScriptableObject.bananaType] += quantity;
            
            var bananaItem = ObjectsReference.Instance.uiBananaSelector.uiBananaSelectorSlots[bananasDataScriptableObject.bananaType];
            bananaItem.gameObject.SetActive(true);
            bananaItem.SetQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);

            ObjectsReference.Instance.bananaMan.SetActiveItem(bananasDataScriptableObject);
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(bananasDataScriptableObject, quantity);

            foreach (var monkey in ObjectsReference.Instance.worldData.monkeys) {
                monkey.SearchForBananaManBananas();
            }
        }

        public int GetQuantity(BananaType bananaType) {
            return bananasInventory[bananaType];
        }

        public void RemoveQuantity(BananaType bananaType, int quantity) {
            var bananaItem = ObjectsReference.Instance.uiBananaSelector.uiBananaSelectorSlots[bananaType];

            if (bananasInventory[bananaType] > quantity) {
                bananasInventory[bananaType] -= quantity;
                bananaItem.SetQuantity(bananasInventory[bananaType]);
            }

            else {
                bananasInventory[bananaType] = 0;
                bananaItem.SetQuantity(0);
                bananaItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiFlippers.SetBananaQuantity(bananasInventory[bananaType]);
        }
    }
}