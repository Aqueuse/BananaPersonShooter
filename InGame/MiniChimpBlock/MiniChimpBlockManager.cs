using System;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MiniChimpBlock {
    public class MiniChimpBlockManager : MonoBehaviour {
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;
        
        private Camera mainCamera;

        private Ray ray;
        private RaycastHit raycastHit;

        public bool isScanningActivated;
        
        private void OnEnable() {
            mainCamera = Camera.main;
        }

        private void Update() {
            if (!isScanningActivated) return;

            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(ray, out raycastHit, Single.PositiveInfinity, layerMask: GestionViewSelectableLayerMask)) {
                ObjectsReference.Instance.uiDescriptionsManager.SetDescription(raycastHit.transform.GetComponent<Tag>().itemScriptableObject, raycastHit.transform.gameObject);
            }
        }
    }
}
