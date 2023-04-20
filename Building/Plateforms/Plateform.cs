using Bananas;
using Enums;
using UnityEngine;

namespace Building.Plateforms {
    public class Plateform : MonoBehaviour {
        [SerializeField] private Material normalPlateformMaterial;
        
        [SerializeField] private Color emissionColor;
        [SerializeField] private Color unactiveColor;

        private UpDownEffect upDownEffect;
        private MeshRenderer _meshRenderer;
        private AudioSource audioSource;

        private Material[] plateformMaterials;
        
        public ItemType plateformType;

        private bool isPlayerOn;
        
        private static readonly int AlimentationColor = Shader.PropertyToID("Alimentation_Color");
        private static readonly int EmissionColor = Shader.PropertyToID("Emission_Color");

        private void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            audioSource = GetComponent<AudioSource>();
            upDownEffect = GetComponent<UpDownEffect>();

            plateformMaterials = new Material[1];

            plateformType = ItemType.EMPTY;
        }

        private void OnCollisionEnter(Collision other) {
            if (!other.gameObject.CompareTag("Banana")) return;
            ActivePlateform(other.gameObject.GetComponent<Banana>().bananasDataScriptableObject.itemType);
        }
        
        private void SetActivatedMaterial(Color color) {
            normalPlateformMaterial.SetColor(AlimentationColor, color);
            normalPlateformMaterial.SetColor(EmissionColor, emissionColor);

            plateformMaterials = new Material[1];
            plateformMaterials[0] = normalPlateformMaterial;

            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.materials = plateformMaterials;
        }

        public void SetUnactiveMaterial() {
            normalPlateformMaterial.SetColor(AlimentationColor, unactiveColor);
            normalPlateformMaterial.SetColor(EmissionColor, unactiveColor);

            plateformMaterials = new Material[1];
            plateformMaterials[0] = normalPlateformMaterial;

            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.materials = plateformMaterials;
        }

        public void ActivePlateform(ItemType itemType) {
            _meshRenderer = GetComponent<MeshRenderer>();
            audioSource = GetComponent<AudioSource>();
            upDownEffect = GetComponent<UpDownEffect>();

            switch (itemType) {
                case ItemType.CAVENDISH:
                    SetActivatedMaterial(ObjectsReference.Instance.scriptableObjectManager.GetBananaScriptableObject(ItemType.CAVENDISH).bananaMaterial.color);
                    audioSource.enabled = true;
                    upDownEffect.isActive = true;

                    plateformType = ItemType.CAVENDISH;
                    ObjectsReference.Instance.mapsManager.currentMap.RefreshPlateformsDataMap();
                    break;
            }
        }
    }
}