using Audio;
using Enums;
using Game;
using UI.InGame;
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
                    Inventory.Instance.AddQuantity(ItemThrowableType.DEBRIS, 1);
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                    MapsManager.Instance.currentMap.Clean();
                    MapsManager.Instance.currentMap.isDiscovered = true;
                    
                    UIQueuedMessages.Instance.AddMessage(
                        "+ 1 "+
                        LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("debris"));
                    
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy() {
            if (MapsManager.Instance != null) return;

            MapsManager.Instance.currentMap.RefreshDebrisDataMap();
        }

        public void DissolveMe() {
            _isDissolving = true;
        }

        public Transform GetTransform() {
            return gameObject.transform;
        }
    }
}
