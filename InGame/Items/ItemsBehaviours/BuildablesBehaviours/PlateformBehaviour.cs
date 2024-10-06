using InGame.Items.ItemsData.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class PlateformBehaviour : BuildableBehaviour {
        [SerializeField] private Color unactiveColor;
        
        private UpEffect upEffect;
        private AudioSource _audioSource;
        
        private bool _isPlayerOn;
        
        private void Start() {
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();
            
            if (isBreaked) BreakBuildable();
            
            _audioSource.enabled = true;
            upEffect.enabled = true;
        }
        
        public void Activate(Rigidbody rigidbody, float force) {
            GetComponent<UpEffect>().Activate(rigidbody, force);
        }
        
        public override void GenerateSaveData() {
            BuildableData plateformData = new BuildableData {
                buildableGuid = buildableGuid,
                buildableType = BuildableType.BUMPER,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.BUMPER, JsonConvert.SerializeObject(plateformData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            BuildableData buildableData = JsonConvert.DeserializeObject<BuildableData>(stringifiedJson);

            buildableGuid = buildableData.buildableGuid;
            buildableType = BuildableType.BUMPER;
            isBreaked = buildableData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( buildableData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(buildableData.buildableRotation);
        }
    }
}