using Enums;
using UnityEngine;

namespace Data {
    [CreateAssetMenu (fileName = "Data", menuName = "ScriptableObjects/UIItemsDataScriptableObject", order = 4)]
    public class ItemsDataScriptableObject : ScriptableObject {
        public GenericDictionary<ItemThrowableType, Sprite> itemSpriteByItemType;

        public GenericDictionary<ItemThrowableType, ItemThrowableCategory> itemsThrowableCategoriesByType;
        
        public GenericDictionary<PlateformType, ItemThrowableType> plateformsTypeByItemThrowableType;
    }
}