using Bananas;
using Data.Bananas;
using Enums;
using Tags;
using UnityEngine;

namespace Gestion.Buildables.Plateforms {
    public class Plateform : MonoBehaviour {
        private Material _normalPlateformMaterial;
        
        [SerializeField] private Color emissionColor;
        [SerializeField] private Color unactiveColor;

        [SerializeField] private SpriteRenderer upDownEffectVizualisation;

        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject workbench;
        
        private UpEffect upEffect;
        private AudioSource _audioSource;

        private Material[] _plateformMaterials;

        public BananaType plateformType;

        private bool _isPlayerOn;
        public bool isWorkBenchActivated;

        private static readonly int AlimentationColor = Shader.PropertyToID("_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("Emission_Color");
        private static readonly int Close = Animator.StringToHash("close");
        private static readonly int Open = Animator.StringToHash("open");
        
        private void Start() {
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();

            _plateformMaterials = new Material[1];
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.GetComponent<Tag>() == null) return;
            if (other.gameObject.GetComponent<Tag>().gameObjectTag != GAME_OBJECT_TAG.BANANA) return;
            
            ActivePlateform(other.gameObject.GetComponent<Banana>().bananasDataScriptableObject);
            ObjectsReference.Instance.mapsManager.currentMap.RefreshItemsDataMap();
        }
        
        private void SetActivatedMaterial(Color color) {
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];
            
            _normalPlateformMaterial.SetColor(AlimentationColor, color);
            _normalPlateformMaterial.SetColor(EmissionColor, emissionColor);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            GetComponent<MeshRenderer>().materials = _plateformMaterials;
        }

        private void SetUnactiveMaterial() {
            _normalPlateformMaterial = GetComponent<MeshRenderer>().materials[0];

            _normalPlateformMaterial.SetColor(AlimentationColor, unactiveColor);
            _normalPlateformMaterial.SetColor(EmissionColor, unactiveColor);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            GetComponent<MeshRenderer>().materials = _plateformMaterials;
        }

        public void ActivePlateform(BananasDataScriptableObject bananasDataScriptableObject) {
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();
            
            switch (bananasDataScriptableObject.bananaType) {
                case BananaType.CAVENDISH:
                    SetActivatedMaterial(bananasDataScriptableObject.bananaColor);
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
            
            SetUnactiveMaterial();

            isWorkBenchActivated = false;
        }
    }
}