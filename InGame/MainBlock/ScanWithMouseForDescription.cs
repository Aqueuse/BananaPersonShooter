using System;
using Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InGame.MainBlock {
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
                var itemData = raycastHit.transform.GetComponent<Tag>().itemScriptableObject;
                
                ObjectsReference.Instance.uiToolTipOnMouseHover.SetDescriptionAndNameInWorldPosition(
                    itemData,
                    Mouse.current.position.value
                    );
                
                enabled = false;
            }
        }
    }
}
