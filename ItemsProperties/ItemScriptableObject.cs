using Tags;
using UnityEngine;

namespace ItemsProperties {
    public class ItemScriptableObject : ScriptableObject {
        public GAME_OBJECT_TAG gameObjectTag;
        
        public ItemCategory itemCategory;
        public BananaType bananaType;
        public RawMaterialType rawMaterialType;
        public IngredientsType ingredientsType;
        public BuildableType buildableType;
        
        public string[] itemName;
    
        public Sprite itemSprite;
        [Multiline] public string[] itemDescription;

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
