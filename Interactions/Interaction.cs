using System.Collections.Generic;
using Gestion.Buildables;
using Enums;
using TMPro;
using UnityEngine;

namespace Interactions {
    public class Interaction : MonoBehaviour {
        [SerializeField] private List<TextMeshPro> interactionTexts;
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
        }

        public void ShowUI() {
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

            if (interactionType == InteractionType.BANANA_DRYER_PLACE_BANANA_PEEL) {
                ObjectsReference.Instance.inputManager.bananasDryerAction.enabled = true;
                ObjectsReference.Instance.inputManager.bananasDryerAction.activeBananaDryer = GetComponent<BananasDryer>();
            }
        }

        public void Desactivate() {
            _isActive = false;

            HideUI();

            if (interactionType == InteractionType.BANANA_DRYER_PLACE_BANANA_PEEL) {
                ObjectsReference.Instance.inputManager.bananasDryerAction.enabled = false;
                ObjectsReference.Instance.inputManager.bananasDryerAction.activeBananaDryer = null;
            }
        }
    }
}
