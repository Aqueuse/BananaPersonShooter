using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Interactions {
    public class Interaction : MonoBehaviour {
        [SerializeField] private List<TextMeshPro> interactionTexts;
        [SerializeField] private SpriteRenderer icon;
        [SerializeField] private bool canRotate;

        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;

        public InteractionType interactionType;

        private void Start() {
            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;
        }

        private void Update() {
            if (!canRotate) return;

            foreach (var interactionText in interactionTexts) {
                interactionText.transform.LookAt(_cameraTransform);
            }
            
            icon.transform.LookAt(_cameraTransform);
        }

        public void ShowUI() {
            foreach (var interactionText in interactionTexts) {
                interactionText.alpha = 1;
            }

            icon.enabled = false;
        }

        public void HideUI() {
            foreach (var interactionText in interactionTexts) {
                interactionText.alpha = 0;
            }
            
            icon.enabled = true;
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
