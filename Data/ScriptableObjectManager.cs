using Data.Bananas;
using Enums;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoBehaviour {
        public MeshReferenceScriptableObject _meshReferenceScriptableObject;
        
        public string GetDescription(ItemCategory itemCategory, int langageIndex, ItemType itemType = ItemType.EMPTY, BuildableType buildableType = BuildableType.EMPTY) {
            if (itemCategory == ItemCategory.BUILDABLE) return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].itemDescription[langageIndex];

            if (itemCategory == ItemCategory.RAW_MATERIAL)
                return _meshReferenceScriptableObject.rawMaterialDataScriptableObjects[itemType].itemDescription[langageIndex];
            
            return _meshReferenceScriptableObject.bananasDataScriptableObject[itemType].itemDescription[langageIndex];
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

        public bool IsBuildable(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.buildableTypeByMesh.ContainsKey(sharedMesh);
        }

        public bool IsDebris(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.debrisPrefabIndexByMesh.ContainsKey(sharedMesh);
        }

        public BuildableType GetBuildableTypeByMesh(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.buildableTypeByMesh[sharedMesh];
        }
    }
}
