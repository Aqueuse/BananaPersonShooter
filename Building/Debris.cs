using Enums;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Debris : MonoBehaviour {
        [SerializeField] private float dissolved;
        public int prefabIndex;

        private static readonly int DissolveProperty = Shader.PropertyToID("Cutoff_Height");
        private float _dissolve;

        private bool _isDissolving;
        
        private void Start() {
            _dissolve = 1.5f;
        }

        private void Update() {
            if (_isDissolving) {
                _dissolve -= Time.deltaTime;
                GetComponent<MeshRenderer>().materials[0].SetFloat(DissolveProperty, _dissolve);
                
                if (_dissolve < dissolved) {
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.METAL, 2);
                    ObjectsReference.Instance.inventory.AddQuantity(ItemCategory.RAW_MATERIAL, ItemType.ELECTRONIC, 1);
                    ObjectsReference.Instance.audioManager.StopAudioSource(AudioSourcesType.EFFECT);
                    ObjectsReference.Instance.mapsManager.currentMap.RecalculateHappiness();
                    ObjectsReference.Instance.mapsManager.currentMap.isDiscovered = true;

                    Destroy(gameObject);
                }
            }
        }

        public void DissolveMe() {
            _isDissolving = true;
        }
    }
}
