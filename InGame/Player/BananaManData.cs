using InGame.Inventories;
using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public DroppedType activeDropped = DroppedType.BANANA;

        public ItemScriptableObject activeDroppableItem;

        public GenericDictionary<DroppedType, Inventory> inventoriesByDroppedType;

        public BananaType activeBanana = BananaType.CAVENDISH;
        public RawMaterialType activeRawMaterial = RawMaterialType.METAL;
        public IngredientsType activeIngredient = IngredientsType.BANANA_DOG_BREAD;
        public ManufacturedItemsType activeManufacturedItem = ManufacturedItemsType.SPACESHIP_TOY;
        public FoodType activeFood = FoodType.BANANA_DOG;
        public BuildableType activeBuildable = BuildableType.BUMPER;
        
        public int bitKongQuantity;

        public int GetActiveDroppableItemQuantity() {
            return inventoriesByDroppedType[activeDropped].GetQuantity(activeDroppableItem);
        }

        public void RemoveActiveDroppableItemQuantity(int quantity) {
            inventoriesByDroppedType[activeDropped].RemoveQuantity(activeDroppableItem, quantity);
        }
    }
}
