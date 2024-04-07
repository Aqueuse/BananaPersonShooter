using InGame.Bananas;
using InGame.Items.ItemsBehaviours.BuildablesBehaviours.PlateformsEffects;
using InGame.Items.ItemsData.BuildablesData;
using Newtonsoft.Json;
using Save.Helpers;
using Tags;
using UnityEngine;

namespace InGame.Items.ItemsBehaviours.BuildablesBehaviours {
    public class PlateformBehaviour : BuildableBehaviour {
        private Material _normalPlateformMaterial;

        [SerializeField] private Color unactiveColor;

        [SerializeField] private SpriteRenderer upDownEffectVizualisation;

        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject workbench;
        [SerializeField] private GameObject plateformInteraction;

        private UpEffect upEffect;
        private AudioSource _audioSource;

        private Material[] _plateformMaterials;

        public BananaType plateformType;

        private bool _isPlayerOn;
        public bool isWorkBenchActivated;

        private static readonly int AlimentationColor = Shader.PropertyToID("_Color");
        private static readonly int Close = Animator.StringToHash("close");
        private static readonly int Open = Animator.StringToHash("open");

        private void Start() {
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();

            _plateformMaterials = new Material[1];
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];
            
            if (isBreaked) BreakBuildable();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Tag>() == null) return;
            if (other.gameObject.GetComponent<Tag>().gameObjectTag != GAME_OBJECT_TAG.BANANA) return;
            
            ActivePlateform(other.gameObject.GetComponent<Banana>().bananasDataScriptableObject.bananaType);
        }

        private void SetActivatedMaterial(Color color) {
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];

            _normalPlateformMaterial.SetColor(AlimentationColor, color);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            GetComponent<MeshRenderer>().materials = _plateformMaterials;
        }

        private void SetUnactiveMaterial() {
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];
            _normalPlateformMaterial.SetColor(AlimentationColor, unactiveColor);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            GetComponent<MeshRenderer>().materials = _plateformMaterials;
        }

        public void ActivePlateform(BananaType bananaType) {
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();

            var bananaData = ObjectsReference.Instance.meshReferenceScriptableObject.bananasPropertiesScriptableObjects[bananaType];

            switch (bananaType) {
                case BananaType.CAVENDISH:
                    SetActivatedMaterial(bananaData.bananaColor);
                    _audioSource.enabled = true;
                    upEffect.isActive = true;

                    plateformType = BananaType.CAVENDISH;
                    upDownEffectVizualisation.enabled = true;
                    break;
            }
        }

        private void Desactivate() {
            plateformType = BananaType.EMPTY;
            _audioSource.enabled = false;
            upEffect.isActive = false;
            upDownEffectVizualisation.enabled = false;
        }

        public void ShowHideWorkbench() {
            if (!isWorkBenchActivated) {
                StartShowingBenchWork();
            }
            else {
                StartHidingWorkbench();
            }
        }

        private void StartShowingBenchWork() {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<MeshCollider>().isTrigger = true;

            plateformInteraction.SetActive(false);

            workbench.SetActive(true);
            _animator.SetTrigger(Open);

            Desactivate();

            isWorkBenchActivated = true;
        }

        private void StartHidingWorkbench() {
            _animator.SetTrigger(Close);
        }

        public void FinishHidingWorkbench() {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshCollider>().isTrigger = false;

            workbench.SetActive(false);
            plateformInteraction.SetActive(true);

            SetUnactiveMaterial();

            isWorkBenchActivated = false;
        }

        public override void GenerateSaveData() {
            PlateformData plateformData = new PlateformData {
                buildableGuid = buildableGuid,
                buildableType = BuildableType.PLATEFORM,
                isBreaked = isBreaked,
                buildablePosition = JsonHelper.FromVector3ToString(transform.position),
                buildableRotation = JsonHelper.FromQuaternionToString(transform.rotation),
                
                bananaType = plateformType
            };

            ObjectsReference.Instance.gameSave.buildablesSave.AddBuildableToBuildableDictionnary(BuildableType.PLATEFORM, JsonConvert.SerializeObject(plateformData));
        }

        public override void LoadSavedData(string stringifiedJson) {
            PlateformData plateformData = JsonConvert.DeserializeObject<PlateformData>(stringifiedJson);

            buildableGuid = plateformData.buildableGuid;
            buildableType = BuildableType.PLATEFORM;
            isBreaked = plateformData.isBreaked;
            transform.position = JsonHelper.FromStringToVector3( plateformData.buildablePosition);
            transform.rotation = JsonHelper.FromStringToQuaternion(plateformData.buildableRotation);
            
            plateformType = plateformData.bananaType;
            
            ActivePlateform(plateformData.bananaType);
        }
    }
}