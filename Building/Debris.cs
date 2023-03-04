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

        private bool isDissolving;

        private void Start() {
            _dissolve = 1.5f;
        }

        private void Update() {
            if (isDissolving) {
                _dissolve -= Time.deltaTime;
                GetComponent<MeshRenderer>().materials[0].SetFloat(DissolveProperty, _dissolve);
                
                if (_dissolve < dissolved) {
                    Inventory.Instance.AddQuantity(ItemThrowableType.DEBRIS, ItemThrowableCategory.CRAFTABLE, 1);
                    AudioManager.Instance.StopAudioSource(AudioSourcesType.EFFECT);
                    MapsManager.Instance.currentMap.Clean();
                    MapsManager.Instance.currentMap.SaveDataOnMap();
                    
                    UIQueuedMessages.Instance.AddMessage(
                        "+ 1 "+
                        LocalizationSettings.Instance.GetStringDatabase().GetLocalizedString("debris"));
                    Destroy(gameObject);
                }
            }
        }

        public void DissolveMe() {
            isDissolving = true;
        }
    }
}
