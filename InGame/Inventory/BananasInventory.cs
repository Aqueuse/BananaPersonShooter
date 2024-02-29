using InGame.Items.ItemsData;
using InGame.Items.ItemsProperties.Bananas;
using UnityEngine;

namespace InGame.Inventory {
    public class BananasInventory : MonoBehaviour {
        private GenericDictionary<BananaType, int> bananasInventory;

        private void Start() {
            bananasInventory = ObjectsReference.Instance.bananaMan.inventories.bananasInventory;
            ObjectsReference.Instance.uiBananasInventory.inventoryScriptableObject = ObjectsReference.Instance.bananaMan.inventories;
        }

        public void AddQuantity(BananasPropertiesScriptableObject bananasDataScriptableObject, int quantity) {
            if (bananasInventory[bananasDataScriptableObject.bananaType] > 10000) return;
            
            bananasInventory[bananasDataScriptableObject.bananaType] += quantity;
            
            var bananaItem = ObjectsReference.Instance.uiBananasInventory.uInventorySlots[bananasDataScriptableObject.bananaType];
            bananaItem.gameObject.SetActive(true);
            bananaItem.SetQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);
            ObjectsReference.Instance.uiTools.SetBananaQuantity(bananasInventory[bananasDataScriptableObject.bananaType]);

            ObjectsReference.Instance.bananaMan.activeItem = bananasDataScriptableObject;
            
            ObjectsReference.Instance.uiQueuedMessages.AddToInventory(bananasDataScriptableObject, quantity);

            foreach (var monkey in World.Instance.monkeysInMap) {
                monkey.SearchForBananaManBananas();
            }
        }

        public int GetQuantity(BananaType bananaType) {
            return bananasInventory[bananaType];
        }

        public void RemoveQuantity(BananaType bananaType, int quantity) {
            var bananaItem = ObjectsReference.Instance.uiBananasInventory.uInventorySlots[bananaType];

            if (bananasInventory[bananaType] > quantity) {
                bananasInventory[bananaType] -= quantity;
                bananaItem.SetQuantity(bananasInventory[bananaType]);
            }

            else {
                bananasInventory[bananaType] = 0;
                bananaItem.SetQuantity(0);
                bananaItem.gameObject.SetActive(false);
            }
            
            ObjectsReference.Instance.uiTools.SetBananaQuantity(bananasInventory[bananaType]);
        }
    }
}