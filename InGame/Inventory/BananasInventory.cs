using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class BananasInventory : MonoBehaviour {
        public Dictionary<BananaType, int> bananasInventory;
        
        public int AddQuantity(BananaType bananaType, int quantity) {
            bananasInventory.TryAdd(bananaType, 0);
            
            if (bananasInventory[bananaType] > 10000) return bananasInventory[bananaType];
            
            bananasInventory[bananaType] += quantity;

            return bananasInventory[bananaType];
        }

        public int GetQuantity(BananaType bananaType) {
            return bananasInventory[bananaType];
        }

        public int RemoveQuantity(BananaType bananaType, int quantity) {
            if (bananasInventory[bananaType] > quantity) {
                bananasInventory[bananaType] -= quantity;
            }

            else {
                bananasInventory[bananaType] = 0;
            }

            return bananasInventory[bananaType];
        }
    }
}