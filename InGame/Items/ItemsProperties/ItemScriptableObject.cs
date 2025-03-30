using UnityEngine;

namespace InGame.Items.ItemsProperties {
    [CreateAssetMenu (fileName = "Properties", menuName = "ScriptableObjects/itemPropertiesScriptableObject", order = 1)]
    public class ItemScriptableObject : ScriptableObject {
        public ItemCategory itemCategory;
        
        public BananaType bananaType;
        public DroppedType droppedType;
        public IngredientsType ingredientsType;
        public ManufacturedItemsType manufacturedItemsType;
        public RawMaterialType rawMaterialType;
        public BuildableType buildableType;
        public FoodType foodType;

        public bool isAspirable;
        public GameObject prefab;
        
        public string[] itemName;
        public int spriteAtlasIndex;

        public Sprite itemSprite;
        [Multiline] public string[] itemDescription;

        public int bitKongValue;

        public string GetName() {
            var langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;

            return itemName[langageIndex];
        }

        public string GetDescription() {
            var langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;

            return itemDescription[langageIndex];
        }

        public Sprite GetSprite() {
            return itemSprite;
        }
    }
}
