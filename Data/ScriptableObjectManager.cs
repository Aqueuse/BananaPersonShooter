using Data.Bananas;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoBehaviour {
        public MeshReferenceScriptableObject _meshReferenceScriptableObject;
        
        public string GetDescription(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            int langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;

            if (itemCategory == ItemCategory.BUILDABLE) return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].itemDescription[langageIndex];

            if (itemCategory == ItemCategory.RAW_MATERIAL)
                return _meshReferenceScriptableObject.rawMaterialDataScriptableObjects[itemType].itemDescription[langageIndex];
            
            return _meshReferenceScriptableObject.bananasDataScriptableObject[itemType].itemDescription[langageIndex];
        }
        
        public string GetName(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            int langageIndex = ObjectsReference.Instance.gameSettings.languageIndexSelected;
            
            if (itemCategory == ItemCategory.BUILDABLE) return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].itemName[langageIndex];

            if (itemCategory == ItemCategory.RAW_MATERIAL)
                return _meshReferenceScriptableObject.rawMaterialDataScriptableObjects[itemType].itemName[langageIndex];
            
            return _meshReferenceScriptableObject.bananasDataScriptableObject[itemType].itemName[langageIndex];
        }
        

        public BananasDataScriptableObject GetBananaScriptableObject(ItemType itemType) {
            return _meshReferenceScriptableObject.bananasDataScriptableObject[itemType];
        }

        public Sprite GetItemSprite(ItemCategory itemCategory, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            switch (itemCategory) {
                case ItemCategory.BUILDABLE:
                    return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].itemSprite;
                case ItemCategory.BANANA:
                    return _meshReferenceScriptableObject.bananasDataScriptableObject[itemType].itemSprite;
                case ItemCategory.RAW_MATERIAL:
                    return _meshReferenceScriptableObject.rawMaterialDataScriptableObjects[itemType].itemSprite;
            }

            return null;
        }

        public GenericDictionary<ItemType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].rawMaterialsWithQuantity;
        }

        public BuildableGridSize GetBuildableGridSizeByMesh(Mesh sharedMesh) {
            var buildableType = _meshReferenceScriptableObject.buildableTypeByMesh[sharedMesh];
            return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].buildableGridSize;
        }
        
        public BuildableType GetBuildableTypeByMesh(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.buildableTypeByMesh[sharedMesh];
        }

        public GameObject BuildablePrefabByBuildableType(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableType];
        }
    }
}
