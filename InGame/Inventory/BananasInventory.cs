using System.Collections.Generic;
using UnityEngine;

namespace InGame.Inventory {
    public class BananasInventory : MonoBehaviour {
        public Dictionary<BananaType, int> bananasInventory = new () {
            {BananaType.BARANGAN, 0},
            {BananaType.BLUE_JAVA, 0},
            {BananaType.BURRO, 0},
            {BananaType.CAVENDISH, 0},
            {BananaType.GOLD_FINGER, 0},
            {BananaType.GROS_MICHEL, 0},
            {BananaType.LADY_FINGER, 0},
            {BananaType.MATOKE, 0},
            {BananaType.MUSA_VELUTINA, 0},
            {BananaType.NANJANGUD, 0},
            {BananaType.PISANG_RAJA, 0},
            {BananaType.PLANTAIN, 0},
            {BananaType.PRAYING_HANDS, 0},
            {BananaType.RED, 0},
            {BananaType.RINO_HORN, 0},
            {BananaType.TINDOK, 0}
        };
        
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

        public void ResetInventory() {
            bananasInventory = new () {
                {BananaType.BARANGAN, 0},
                {BananaType.BLUE_JAVA, 0},
                {BananaType.BURRO, 0},
                {BananaType.CAVENDISH, 0},
                {BananaType.GOLD_FINGER, 0},
                {BananaType.GROS_MICHEL, 0},
                {BananaType.LADY_FINGER, 0},
                {BananaType.MATOKE, 0},
                {BananaType.MUSA_VELUTINA, 0},
                {BananaType.NANJANGUD, 0},
                {BananaType.PISANG_RAJA, 0},
                {BananaType.PLANTAIN, 0},
                {BananaType.PRAYING_HANDS, 0},
                {BananaType.RED, 0},
                {BananaType.RINO_HORN, 0},
                {BananaType.TINDOK, 0}
            };
        }
    }
}