using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventories {
    public class BananaGoopInventory : MonoBehaviour {
        public Dictionary<BananaEffect, int> bananaGoopInventory = new () {
            {BananaEffect.ATTRACTION, 0},
            {BananaEffect.REPULSION, 0},
            {BananaEffect.SLOW, 0},
            {BananaEffect.FAST, 0}
        };
        
        public int AddQuantity(BananaEffect bananaEffect, int quantity) {
            bananaGoopInventory.TryAdd(bananaEffect, 0);
            
            if (bananaGoopInventory[bananaEffect] > 100) return bananaGoopInventory[bananaEffect];
            
            bananaGoopInventory[bananaEffect] += quantity;

            return bananaGoopInventory[bananaEffect];
        }

        public int GetQuantity(BananaEffect bananaType) {
            return bananaGoopInventory[bananaType];
        }

        public int RemoveQuantity(BananaEffect bananaEffect, int quantity) {
            if (bananaGoopInventory[bananaEffect] > quantity) {
                bananaGoopInventory[bananaEffect] -= quantity;
            }

            else {
                bananaGoopInventory[bananaEffect] = 0;
            }

            return bananaGoopInventory[bananaEffect];
        }

        public void ResetInventory() {
            bananaGoopInventory = new () {
                {BananaEffect.ATTRACTION, 0},
                {BananaEffect.REPULSION, 0},
                {BananaEffect.SLOW, 0},
                {BananaEffect.FAST, 0}
            };
        }
    }
}