using System.Collections.Generic;
using Data.Bananas;
using Enums;
using UnityEngine;

namespace Data {
    public class ScriptableObjectManager : MonoBehaviour {
        public MeshReferenceScriptableObject _meshReferenceScriptableObject;

        public List<BuildableType> buildablesToGive;
        
        public BananasDataScriptableObject GetBananaScriptableObject(BananaType bananaType) {
            return _meshReferenceScriptableObject.bananasDataScriptableObjects[bananaType];
        }
        
        public GenericDictionary<RawMaterialType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            var buildableDataScriptableObject = _meshReferenceScriptableObject.buildableDataScriptableObjects[buildableType];

            return buildableDataScriptableObject.rawMaterialsWithQuantity;
        }
        
        public BuildableType GetBuildableTypeByMesh(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.buildableTypeByMesh[sharedMesh];
        }

        public GameObject BuildablePrefabByBuildableType(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableType];
        }
    }
}
