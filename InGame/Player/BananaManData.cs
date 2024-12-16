using InGame.Items.ItemsProperties;
using UnityEngine;

namespace InGame.Player {
    public class BananaManData : MonoBehaviour {
        public DroppedType activeDropped = DroppedType.BANANA;
        
        public ItemScriptableObject activeDroppableItem;
        
        public BananaType activeBanana = BananaType.CAVENDISH;
        public RawMaterialType activeRawMaterial = RawMaterialType.METAL;
        public IngredientsType activeIngredient = IngredientsType.BANANA_DOG_BREAD;
        public ManufacturedItemsType activeManufacturedItem = ManufacturedItemsType.SPACESHIP_TOY;
        public BuildableType activeBuildable = BuildableType.BUMPER;
        public FoodType activeFood = FoodType.BANANA_DOG;
        
        public int bitKongQuantity;
    }
}
