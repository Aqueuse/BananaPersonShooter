using InGame.Monkeys.PhysicToNavMeshCoordinations;
using Newtonsoft.Json;
using Save.Buildables;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class BumperBehaviour : BuildableBehaviour {
        [SerializeField] private Color unactiveColor;
        
        private AudioSource _audioSource;

        private bool _isPlayerOn;

        private void Start() {
            _audioSource = GetComponent<AudioSource>();

            if (buildableData.buildableState == BuildableState.BLUEPRINT) {
                ChangeToBlueprint();
            }
            else {
                ChangeToCompleted();
            }
            
            _audioSource.enabled = true;
        }
        
        public void Activate(Rigidbody rigidbody, float force) {
            if (buildableData.buildableState == BuildableState.BLUEPRINT) return;
            
            if (rigidbody.transform.TryGetComponent(out PhysicNavMeshCoordination physicNavMeshCoordination)) physicNavMeshCoordination.SwitchToPhysic();
            rigidbody.AddForce(transform.up * force, ForceMode.Acceleration);
        }
        
        public override void GenerateSaveData() {
            var plateformData = new BuildableSavedData {
                buildableGuid = buildableData.buildableGuid,
                buildableType = BuildableType.BUMPER,
                buildableState = buildableData.buildableState,
                buildingMaterials = buildableData.buildingMaterials,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation)
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.BUMPER, JsonConvert.SerializeObject(plateformData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            var buildableSavedData = JsonConvert.DeserializeObject<BuildableSavedData>(stringifiedJson);
            buildableData.buildableGuid = buildableSavedData.buildableGuid;
            
            buildableData.buildableState = buildableSavedData.buildableState;
            buildableData.buildingMaterials = buildableSavedData.buildingMaterials;
            
            transform.position = JsonHelper.FromStringToVector3( buildableSavedData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(buildableSavedData.buildableRotation);
        }
    }
}