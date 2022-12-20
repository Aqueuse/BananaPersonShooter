using System.Collections.Generic;
using Enums;

public class Inventory : MonoSingleton<Inventory> {
    public Dictionary<BananaType, int> bananaManInventory;
    
    private void Start() {
        bananaManInventory = new Dictionary<BananaType, int> {
            {BananaType.RED, 60},
            {BananaType.BURRO, 60},
            {BananaType.MATOKE, 60},
            {BananaType.TINDOK, 60},
            {BananaType.MANZANO, 60},
            {BananaType.BARANGAN, 60},
            {BananaType.BLUE_JAVA, 60},
            {BananaType.PLANTAIN, 60},
            {BananaType.RINO_HORN, 60},
            {BananaType.CAVENDISH, 60},
            {BananaType.GROS_MICHEL, 60},
            {BananaType.LADY_FINGER, 60},
            {BananaType.PISANG_RAJA, 60},
            {BananaType.PRAYING_HANDS, 60},
            {BananaType.GOLD_FINGER, 60},
            {BananaType.NANJANGUD, 60},
            {BananaType.EMPTY_HAND, 1}
        };
    }

    public void AddQuantity(BananaType bananaType, int quantity) {
        bananaManInventory[bananaType] += quantity;
    }

    public int GetQuantity(BananaType bananaType) {
        return bananaManInventory[bananaType];
    }

    public void RemoveQuantity(BananaType bananaType, int quantity) {
        if (bananaManInventory[bananaType] > quantity) bananaManInventory[bananaType] -= quantity;
        else {
            bananaManInventory[bananaType] = 0;
        }
    }
}