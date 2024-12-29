using System;
using System.Collections.Generic;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BuildableBehaviour : MonoBehaviour {
        public string buildableGuid;
        public BuildableType buildableType;
        public bool isBreaked;
        
        public Transform ChimpTargetTransform;
        public Transform ChimpTargetLookAtTransform;

        public bool isVisitorTargetable;
        public BuildablePropertiesScriptableObject buildablePropertiesScriptableObject; 
        
        public bool isPirateTargeted;
        public bool isVisitorTargeted;

        [SerializeField] private List<MeshRenderer> meshRenderersToBreak;
        private static readonly int crackOpacity = Shader.PropertyToID("_crack_opacity");

        private void Start() {
            if(string.IsNullOrEmpty(buildableGuid)) {
                buildableGuid = Guid.NewGuid().ToString();
            }
            
            if (isBreaked) BreakBuildable();
        }

        public void BreakBuildable() {
            isBreaked = true;
            isPirateTargeted = false;

            foreach (var meshRenderer in meshRenderersToBreak) {
                meshRenderer.material.SetFloat(crackOpacity, 1);
            }
        }

        public void RepairBuildable() {
            isBreaked = false;
            isPirateTargeted = false;

            foreach (var craftingIngredient in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                ObjectsReference.Instance.bananaManRawMaterialInventory.RemoveQuantity(
                    craftingIngredient.Key, 
                    craftingIngredient.Value);
                
                ObjectsReference.Instance.uiQueuedMessages.RemoveFromInventory(
                    craftingIngredient.Key, 
                    craftingIngredient.Value
                );
                ObjectsReference.Instance.uiFlippers.RefreshActiveBuildableAvailability();
            }
            
            foreach (var meshRenderer in meshRenderersToBreak) {
                meshRenderer.material.SetFloat(crackOpacity, 0);
            }
        }

        public virtual void RetrieveRawMaterials() {
            foreach (var craftingMaterial in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                for (int i = 0; i < craftingMaterial.Value; i++) {
                    Instantiate(craftingMaterial.Key.prefab);
                }
            }
        }
        
        public virtual void GenerateSaveData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
