using ItemsProperties.Bananas;
using UnityEngine;

namespace ItemsProperties {
    public class ScriptableObjectManager : MonoBehaviour {
        public MeshReferenceScriptableObject _meshReferenceScriptableObject;
        
        public BananasPropertiesScriptableObject GetBananaScriptableObject(BananaType bananaType) {
            return _meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType];
        }

        public GenericDictionary<RawMaterialType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            var buildablePropertiesScriptableObject = _meshReferenceScriptableObject.buildablePropertiesScriptableObjects[buildableType];

            return buildablePropertiesScriptableObject.rawMaterialsWithQuantity;
        }

        public GameObject BuildablePrefabByBuildableType(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableType];
        }
    }
}
