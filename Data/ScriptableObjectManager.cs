using System.Collections.Generic;
using Data.Bananas;
using Data.Buildables;
using Data.RawMaterial;
using Enums;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<ItemType, BananasDataScriptableObject> bananasDataScriptableObject;
        [SerializeField] private GenericDictionary<BuildableType, BuildableDataScriptableObject> buildablesDataScriptableObject;
        [SerializeField] private GenericDictionary<ItemType, RawMaterialDataScriptableObject> rawMaterialDataScriptableObjects;
        
        public string GetDescription(ItemCategory itemCategory, int langageIndex, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            if (itemCategory == ItemCategory.BUILDABLE) return buildablesDataScriptableObject[buildableType].itemDescription[langageIndex];

            if (itemCategory == ItemCategory.RAW_MATERIAL)
                return rawMaterialDataScriptableObjects[itemType].itemDescription[langageIndex];
            
            return bananasDataScriptableObject[itemType].itemDescription[langageIndex];
        }

        public BananasDataScriptableObject GetBananaScriptableObject(ItemType itemType) {
            return bananasDataScriptableObject[itemType];
        }

        public Sprite GetItemSprite(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            switch (itemCategory) {
                case ItemCategory.BUILDABLE:
                    return buildablesDataScriptableObject[buildableType].itemSprite;
                case ItemCategory.BANANA:
                    return bananasDataScriptableObject[itemType].itemSprite;
                case ItemCategory.RAW_MATERIAL:
                    return rawMaterialDataScriptableObjects[itemType].itemSprite;
            }

            return null;
        }

        public GenericDictionary<ItemType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            return buildablesDataScriptableObject[buildableType].rawMaterialsWithQuantity;
        }
    }
}
