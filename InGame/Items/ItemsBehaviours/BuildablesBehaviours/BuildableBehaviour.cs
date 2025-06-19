using System;
using System.Collections.Generic;
using System.Linq;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using InGame.Items.ItemsData.Buildables;
using InGame.Items.ItemsProperties.Buildables;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BuildableBehaviour : MonoBehaviour {
        public BuildableData buildableData;
        
        public Transform ChimpTargetTransform;
        public Transform ChimpTargetLookAtTransform;

        public bool isVisitorTargetable;
        public BuildablePropertiesScriptableObject buildablePropertiesScriptableObject; 
        
        public bool isPirateTargeted;
        public bool isVisitorTargeted;

        [SerializeField] private List<MeshRenderer> meshRenderers;

        public void Init() {
            if(string.IsNullOrEmpty(buildableData.buildableGuid)) {
                buildableData.buildableGuid = Guid.NewGuid().ToString();
            }

            if (buildableData.buildingMaterials.Count == 0) {
                foreach (var rawMaterial in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                    buildableData.buildingMaterials.Add(rawMaterial.Key.rawMaterialType, 0);
                }
            }
            
            if (buildableData.buildableState == BuildableState.BLUEPRINT) {
                ChangeToBlueprint();
            }
            else {
                ChangeToCompleted();
            }
        }
        
        public void TryAbsorbeRawMaterial(DroppedBehaviour droppedBehaviour) {
            if (ObjectsReference.Instance.aspireAction.isAspiring) return;
            if (buildableData.buildableState != BuildableState.BLUEPRINT) return;

            var rawMaterialType = droppedBehaviour.itemScriptableObject.rawMaterialType;
            
            foreach (var rawMaterial in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                if (rawMaterial.Key.rawMaterialType == rawMaterialType) {
                    if (buildableData.buildingMaterials[rawMaterialType] < rawMaterial.Value) {
                        buildableData.buildingMaterials[rawMaterialType] += 1;
                        Destroy(droppedBehaviour.gameObject);
                        ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.TAKE_SOMETHING, 0);

                        if (IsBuildableCompleted()) {
                            ChangeToCompleted();
                            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MISSING_MATERIALS_PANEL, false);
                            return;
                        }
                    }
                }
            }
        }

        private bool IsBuildableCompleted() {
            foreach (var rawMaterial in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                if (buildableData.buildingMaterials[rawMaterial.Key.rawMaterialType] != rawMaterial.Value) {
                    return false;
                }
            }

            return true;
        }
        
        public virtual void TryRetrieveOneRawMaterial() {
            var rawMaterialsWithQuantity = buildableData.buildingMaterials.ToArray();
            
            foreach (var craftingMaterial in rawMaterialsWithQuantity) {
                for (var i = 0; i < craftingMaterial.Value; i++) {
                    if (craftingMaterial.Value > 0) {
                        Instantiate(
                            parent: ObjectsReference.Instance.gameSave.savablesItemsContainer, 
                            original: ObjectsReference.Instance.meshReferenceScriptableObject.rawMaterialPrefabByRawMaterialType[craftingMaterial.Key], 
                            position:transform.position, 
                            rotation:transform.rotation
                        );

                        buildableData.buildingMaterials[craftingMaterial.Key] -= 1;
                    }
                }
            }
            
            Destroy(gameObject);
        }
        
        public void ChangeToBlueprint() {
            if (buildableData.buildableState == BuildableState.BLUEPRINT) return;
            
            buildableData.buildableState = BuildableState.BLUEPRINT;
            isPirateTargeted = false;

            foreach (var meshRenderer in meshRenderers) {
                meshRenderer.material = ObjectsReference.Instance.meshReferenceScriptableObject.blueprintBuildableMaterial;
            }
        }

        public void ChangeToCompleted() {
            buildableData.buildableState = BuildableState.COMPLETED;
            isPirateTargeted = false;
            
            foreach (var meshRenderer in meshRenderers) {
                meshRenderer.material = ObjectsReference.Instance.meshReferenceScriptableObject.completedBuildableMaterial;
            }

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MISSING_MATERIALS_PANEL, false);
            GetComponent<MeshCollider>().isTrigger = false;
            GetComponent<MeshCollider>().convex = false;
        }
        
        public List<(Sprite, int)> GetMissingMaterialsWithQuantity() {
            var missingMaterials = new List<(Sprite, int)>();

            foreach (var rawMaterial in buildablePropertiesScriptableObject.rawMaterialsWithQuantity) {
                if (buildableData.buildingMaterials[rawMaterial.Key.rawMaterialType] < rawMaterial.Value) {
                    missingMaterials.Add(
                        new (
                            rawMaterial.Key.GetSprite(),
                            rawMaterial.Value - buildableData.buildingMaterials[rawMaterial.Key.rawMaterialType]
                        )
                    );
                }
            }

            return missingMaterials;
        }
        
        public virtual void GenerateSaveData() { }
        
        public virtual void LoadSavedData(string stringifiedJson) { }
    }
}
