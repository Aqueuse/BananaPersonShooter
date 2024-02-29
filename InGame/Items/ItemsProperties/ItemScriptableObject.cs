using Tags;
using UnityEngine;

namespace InGame.Items.ItemsProperties {
    public class ItemScriptableObject : ScriptableObject {
        public GAME_OBJECT_TAG gameObjectTag;

        public ItemCategory itemCategory;
        public BananaType bananaType;
        public RawMaterialType rawMaterialType;
        public IngredientsType ingredientsType;
        public ManufacturedItemsType manufacturedItemsType;
        public BuildableType buildableType;

        public string[] itemName;

        public Sprite itemSprite;
        [Multiline] public string[] itemDescription;

        public int bitKongValue;

        public string GetName() {
            int langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;

            return itemName[langageIndex];
        }

        public string GetDescription() {
            int langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;

            return itemDescription[langageIndex];
        }

        public Sprite GetSprite() {
            return itemSprite;
        }
    }
}
