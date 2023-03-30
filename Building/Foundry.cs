using Enums;
using Game;
using TMPro;
using UI.InGame;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Building {
    public class Foundry : MonoBehaviour {
        [SerializeField] private GameObject debrisStack;
        [SerializeField] private GameObject lavaStack;
        [SerializeField] private GameObject ingots;

        [SerializeField] private TextMeshProUGUI quantityText;
        
        int _ingotsQuantity;
        private int _debrisQuantity;
        private float _timeToConvert;

        private bool _isHeating;

        private string _ingotString;
        
        private MeshRenderer[] _debrisRenderers; 
        private MeshRenderer[] _ingotsrenderers;
        private AudioSource[] _ingotsAudioSources;
        
        private void Start() {
            _debrisQuantity = 0;
            _ingotsQuantity = 0;
            _timeToConvert = 4f;
            _isHeating = false;
            
            _debrisRenderers = debrisStack.GetComponentsInChildren<MeshRenderer>();
            _ingotsrenderers = ingots.GetComponentsInChildren<MeshRenderer>();
            _ingotsAudioSources = ingots.GetComponentsInChildren<AudioSource>();
        }

        public void Load_One_More_Debris() {
            if (Inventory.Instance.GetQuantity(ItemThrowableType.DEBRIS) > 0 && _debrisQuantity < 8) {
                _debrisQuantity++;
                Inventory.Instance.RemoveQuantity(ItemThrowableType.DEBRIS, 1);
                SetDebrisQuantityText(_debrisQuantity);
                
                for (var i = 0; i < _debrisQuantity; i++) {
                    _debrisRenderers[i].enabled = true;
                }

                if (!_isHeating) {
                    Invoke(nameof(Start_To_Heat), 2f);
                    _isHeating = true;
                }
            }
        }

        private void Start_To_Heat() {
            lavaStack.SetActive(true);
            
            if (_debrisQuantity > 0) {
                 Invoke(nameof(Add_One_more_Ingot), _timeToConvert);
            }
        }
        
        private void Add_One_more_Ingot() {
            if (_ingotsQuantity < 8) {
                ingots.SetActive(true);
                _ingotsQuantity++;
                _debrisQuantity--;
                SetDebrisQuantityText(_debrisQuantity);
                
                foreach (var meshRenderer in _ingotsrenderers) {
                    meshRenderer.enabled = false;
                }

                for (var i = 0; i < _ingotsQuantity; i++) {
                    _ingotsrenderers[i].enabled = true;
                    _ingotsAudioSources[i].Play();
                }
                
                Remove_One_Debris();

                if (_debrisQuantity > 0) {
                    Invoke(nameof(Add_One_more_Ingot), _timeToConvert);
                }

                else {
                    Finish_Heating();
                }
            }
        }

        private void Remove_One_Debris() {
            foreach (var debrisRenderer in _debrisRenderers) {
                if (debrisRenderer.enabled) {
                    debrisRenderer.enabled = false;
                    return;
                }
            }
        }

        private void Finish_Heating() {
            _isHeating = false;
            lavaStack.SetActive(false);
        }

        public void Give_Ingots_To_Player() {
            if (_ingotsQuantity <= 0) return;

            Inventory.Instance.AddQuantity(ItemThrowableType.INGOT, _ingotsQuantity);

            _ingotString = LocalizationSettings.StringDatabase.GetLocalizedString(_ingotsQuantity == 0 ? "ingot" : "ingots");
            UIQueuedMessages.Instance.AddMessage("+ "+ _ingotsQuantity+ " "+ _ingotString);
            
            _ingotsQuantity -= _ingotsQuantity;

            foreach (var meshRenderer in _ingotsrenderers) {
                meshRenderer.enabled = false;
            }
        }
        
        private void SetDebrisQuantityText(int quantity) {
            quantityText.text = "("+quantity+"/8)";
        }
    }
}
