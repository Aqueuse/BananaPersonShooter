using Bananas;
using UnityEngine;

namespace Building.Buildables.Plateforms {
    public class Plateform : MonoBehaviour {
        private Material _normalPlateformMaterial;
        
        [SerializeField] private Color emissionColor;
        [SerializeField] private Color unactiveColor;

        private UpDownEffect _upDownEffect;
        private MeshRenderer _meshRenderer;
        private AudioSource _audioSource;

        private Material[] _plateformMaterials;
        
        public ItemType plateformType;

        private bool _isPlayerOn;
        
        private static readonly int AlimentationColor = Shader.PropertyToID("Alimentation_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("Emission_Color");

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _upDownEffect = GetComponent<UpDownEffect>();

            _plateformMaterials = new Material[1];

            plateformType = ItemType.EMPTY;
            _normalPlateformMaterial = _meshRenderer.materials[0];
        }

        private void OnCollisionEnter(Collision other) {
            if (!other.gameObject.CompareTag("Banana")) return;
            ActivePlateform(other.gameObject.GetComponent<Banana>().bananasDataScriptableObject.itemType);
        }
        
        private void SetActivatedMaterial(Color color) {
            _meshRenderer = GetComponent<MeshRenderer>();
            _normalPlateformMaterial = _meshRenderer.materials[0];
            
            _normalPlateformMaterial.SetColor(AlimentationColor, color);
            _normalPlateformMaterial.SetColor(EmissionColor, emissionColor);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            _meshRenderer.materials = _plateformMaterials;
        }

        public void SetUnactiveMaterial() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _normalPlateformMaterial = _meshRenderer.materials[0];
            
            _normalPlateformMaterial.SetColor(AlimentationColor, unactiveColor);
            _normalPlateformMaterial.SetColor(EmissionColor, unactiveColor);

            _plateformMaterials = new Material[1];
            _plateformMaterials[0] = _normalPlateformMaterial;

            _meshRenderer.materials = _plateformMaterials;
        }

        private void ActivePlateform(ItemType itemType) {
            _meshRenderer = GetComponent<MeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            _upDownEffect = GetComponent<UpDownEffect>();

            switch (itemType) {
                case ItemType.CAVENDISH:
                    SetActivatedMaterial(ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ItemType.CAVENDISH).bananaMaterial.color);
                    _audioSource.enabled = true;
                    _upDownEffect.isActive = true;

                    plateformType = ItemType.CAVENDISH;
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesDataMap();
                    break;
            }
        }
    }
}