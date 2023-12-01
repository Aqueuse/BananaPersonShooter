using Data.Bananas;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoBehaviour {
        public MeshReferenceScriptableObject _meshReferenceScriptableObject;
        
        public BananasDataScriptableObject GetBananaScriptableObject(BananaType bananaType) {
            return _meshReferenceScriptableObject.bananasDataScriptableObjects[bananaType];
        }

        public GenericDictionary<RawMaterialType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            var buildableDataScriptableObject = _meshReferenceScriptableObject.buildableDataScriptableObjects[buildableType];

            return buildableDataScriptableObject.rawMaterialsWithQuantity;
        }

        public GameObject BuildablePrefabByBuildableType(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableType];
        }
    }
}
