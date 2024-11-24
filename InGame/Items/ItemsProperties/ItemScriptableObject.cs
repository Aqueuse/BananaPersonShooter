using Tags;
using UnityEngine;

namespace InGame.Items.ItemsProperties {
    public class ItemScriptableObject : ScriptableObject {
        public GAME_OBJECT_TAG gameObjectTag;

        public ItemCategory itemCategory;
        
        public BananaType bananaType;
        public DroppedType droppedType;
        public IngredientsType ingredientsType;
        public ManufacturedItemsType manufacturedItemsType;
        public RawMaterialType rawMaterialType;
        public BuildableType buildableType;

        public string[] itemName;

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
