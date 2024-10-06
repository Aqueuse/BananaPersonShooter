using System;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MiniChimpBlock {
    public class ScanWithMouseForDescription : MonoBehaviour {
        [SerializeField] private LayerMask GestionViewSelectableLayerMask;
        
        private Camera mainCamera;

        private Ray ray;
        private RaycastHit raycastHit;
        
        private void OnEnable() {
            mainCamera = Camera.main;
        }

        private void Update() {
            ray = mainCamera.ScreenPointToRay(Mouse.current.position.value);

            if (Physics.Raycast(ray, out raycastHit, Single.PositiveInfinity, layerMask: GestionViewSelectableLayerMask)) {
                ObjectsReference.Instance.uiDescriptionsManager.SetDescription(raycastHit.transform.GetComponent<Tag>().itemScriptableObject, raycastHit.transform.gameObject);
                ObjectsReference.Instance.uiManager.ShowMiniChimpBlock();
                ObjectsReference.Instance.uiBananaGun.SwitchToDescription();
                enabled = false;
            }
        }
    }
}
