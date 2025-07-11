using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.InGame.Interactions {
    public class UInteraction : MonoBehaviour {
        [SerializeField] private List<TextMeshPro> interactionTexts;

        private Vector3 _localScale;
        private float _cameraDistance;

        public InteractionType interactionType;
        
        private void ShowUI() {
            foreach (var interactionText in interactionTexts) {
                interactionText.alpha = 1;
            }
        }

        public void HideUI() {
            foreach (var interactionText in interactionTexts) {
                interactionText.alpha = 0;
            }
        }
        
        private bool _isActive;

        public void Activate() {
            if (_isActive) return;

            ShowUI();
            _isActive = true;
        }

        public void Desactivate() {
            _isActive = false;

            HideUI();
        }
    }
}
