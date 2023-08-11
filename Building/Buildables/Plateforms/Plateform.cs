using Bananas;
using Items;
using UnityEngine;

namespace Building.Buildables.Plateforms {
    public class Plateform : MonoBehaviour {
        private Material _normalPlateformMaterial;
        
        [SerializeField] private Color emissionColor;
        [SerializeField] private Color unactiveColor;

        [SerializeField] private MeshRenderer upDownEffectVizualisation;
        
        private UpEffect upEffect;
        private MeshRenderer _meshRenderer;
        private AudioSource _audioSource;

        private Material[] _plateformMaterials;
        
        public ItemType plateformType;

        private bool _isPlayerOn;
        
        private static readonly int AlimentationColor = Shader.PropertyToID("_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("Emission_Color");

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();

            _plateformMaterials = new Material[1];
            _normalPlateformMaterial = _meshRenderer.materials[0];
        }

        private void OnCollisionEnter(Collision other) {
            if (!other.gameObject.CompareTag("Banana")) return;
            ActivePlateform(other.gameObject.GetComponent<Banana>().bananasDataScriptableObject.itemType);
            ObjectsReference.Instance.mapsManager.currentMap.RefreshAspirablesItemsDataMap();
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

        public void ActivePlateform(ItemType itemType) {
            _meshRenderer = GetComponent<MeshRenderer>();
            _audioSource = GetComponent<AudioSource>();
            upEffect = GetComponent<UpEffect>();
            
            switch (itemType) {
                case ItemType.CAVENDISH:
                    SetActivatedMaterial(ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ItemType.CAVENDISH).bananaColor);
                    _audioSource.enabled = true;
                    upEffect.isActive = true;

                    plateformType = ItemType.CAVENDISH;
                    GetComponent<ItemThrowable>().itemType = ItemType.CAVENDISH;
                    upDownEffectVizualisation.enabled = true;
                    break;
            }
        }
    }
}