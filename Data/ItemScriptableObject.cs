using UnityEngine;

namespace Data {
    public class ItemScriptableObject : ScriptableObject {
        public ItemType itemType;
        public ItemCategory itemCategory;
        public BuildableType buildableType;

        public string[] itemName;
    
        public Sprite itemSprite;
        [Multiline] public string[] itemDescription;
    }
}
