using System.Collections.Generic;
using Data;
using Enums;
using UI.InGame.Blueprints;
using UnityEngine;

namespace Items {
    public class BuildablesManager : MonoBehaviour {
        private MeshReferenceScriptableObject _meshReferenceScriptableObject;

        public List<BuildableType> playerBlueprints;

        public List<BuildableType> buildablesToGive;

        private void Start() {
            _meshReferenceScriptableObject = ObjectsReference.Instance.scriptableObjectManager._meshReferenceScriptableObject;
        }
        
        public bool HasBlueprint(BuildableType buildableType) {
            var activeBlueprints = transform.GetComponentsInChildren<UIBlueprintSlot>();

            foreach (var uiBlueprintSlot in activeBlueprints) {
                if (uiBlueprintSlot.buildableType == buildableType) return true;
            }

            return false;
        }
        
        public GenericDictionary<ItemType, int> GetBuildableCraftingIngredients(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablesDataScriptableObject[buildableType].rawMaterialsWithQuantity;
        }
        
        public BuildableType GetBuildableTypeByMesh(Mesh sharedMesh) {
            return _meshReferenceScriptableObject.buildableTypeByMesh[sharedMesh];
        }

        public GameObject BuildablePrefabByBuildableType(BuildableType buildableType) {
            return _meshReferenceScriptableObject.buildablePrefabByBuildableType[buildableType];
        }
    }
}
