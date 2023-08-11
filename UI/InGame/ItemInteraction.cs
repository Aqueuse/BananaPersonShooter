using TMPro;
using UnityEngine;

namespace UI.InGame {
    public class ItemInteraction : MonoBehaviour {
        [SerializeField] private TextMeshPro interactionText;
        [SerializeField] private bool canRotate;

        private Transform _cameraTransform;
        private Vector3 _localScale;
        private float _cameraDistance;

        private void Start() {
            _cameraTransform = ObjectsReference.Instance.mainCamera.transform;
        }

        private void Update() {
            if (!canRotate) return;
            
            interactionText.transform.LookAt(_cameraTransform);
        }

        public void ShowUI() {
            interactionText.alpha = 1;
        }

        public void HideUI() {
            interactionText.alpha = 0;
        }
    }
}
