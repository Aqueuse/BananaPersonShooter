using System;
using InGame.Items.ItemsBehaviours.DroppedBehaviours;
using Newtonsoft.Json;
using Save.Buildables;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class SilicaRefineryBehaviour : BuildableBehaviour {
        [SerializeField] private GameObject silicaPrefab;
        [SerializeField] private GameObject electronicComponentPrefab;
        
        [SerializeField] private Transform componentsOutputLocation;
        
        public int silicaQuantity;
        private readonly int maxQuantity = 80;

        private int conversionTimer;
        private int conversionTime = 5000;
        
        private void Update() {
            if (silicaQuantity <= 0) return;

            conversionTimer -= 1;

            if (conversionTimer <= conversionTime) {
                GiveElectronicComponent();
                conversionTimer = conversionTime;
            }
        }
        
        private void PutSilica() {
            silicaQuantity += 1;
            
        }
        
        private void GiveElectronicComponent() {
            Instantiate(
                electronicComponentPrefab,
                componentsOutputLocation.position,
                componentsOutputLocation.rotation,
                ObjectsReference.Instance.gameSave.savablesItemsContainer
            );
        }
        
        public override void TryRetrieveOneRawMaterial() {
            base.TryRetrieveOneRawMaterial();

            for (int i = 0; i < silicaQuantity; i++) {
                Instantiate(
                    silicaPrefab,
                    transform.position,
                    transform.rotation,
                    ObjectsReference.Instance.gameSave.savablesItemsContainer
                );
            }
        }
        
        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.layer != 7) return;
            if (buildableData.buildableState == BuildableState.BLUEPRINT) return;

            if (other.gameObject.TryGetComponent<DroppedBehaviour>(out var droppedBehaviour)) {
                if (droppedBehaviour.itemScriptableObject.rawMaterialType == RawMaterialType.SILICE) {
                    if (silicaQuantity <= maxQuantity) {
                        PutSilica();
                        Destroy(other.gameObject);
                    }
                }
            }
        }
        
        public override void GenerateSaveData() {
            if(string.IsNullOrEmpty(buildableData.buildableGuid)) {
                buildableData.buildableGuid = Guid.NewGuid().ToString();
            }

            var silicaRefineryData = new SilicaRefinerySavedData {
                buildableGuid = buildableData.buildableGuid,
                buildableType = BuildableType.SILICA_REFINERY,
                
                buildableState = buildableData.buildableState,
                buildingMaterials = buildableData.buildingMaterials,
                
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                conversionTime = conversionTime
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.SILICA_REFINERY, JsonConvert.SerializeObject(silicaRefineryData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            var silicaRefineryData = JsonConvert.DeserializeObject<SilicaRefinerySavedData>(stringifiedJson);
            buildableData.buildableGuid = silicaRefineryData.buildableGuid;
            
            buildableData.buildableState = silicaRefineryData.buildableState;
            buildableData.buildingMaterials = silicaRefineryData.buildingMaterials;

            transform.position = JsonHelper.FromStringToVector3( silicaRefineryData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(silicaRefineryData.buildableRotation);

            conversionTime = silicaRefineryData.conversionTime;
        }
    }
}
