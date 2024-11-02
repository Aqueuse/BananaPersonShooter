using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class CannonsPanelActions : InputActions {
        [SerializeField] private InputActionReference switchToLeftCannonActionReference;
        [SerializeField] private InputActionReference switchToRightCannonActionReference;

        [SerializeField] private InputActionReference shootActionReference;
        [SerializeField] private InputActionReference rotateCannonActionReference;

        [SerializeField] private InputActionReference zoomActionReference;
        
        private void OnEnable() {
            switchToLeftCannonActionReference.action.Enable();
            switchToLeftCannonActionReference.action.performed += SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Enable();
            switchToRightCannonActionReference.action.performed += SwitchToCannonRight;
            
            shootActionReference.action.Enable();
            shootActionReference.action.performed += ShowLaser;
            shootActionReference.action.canceled += HideLaser; 

            rotateCannonActionReference.action.Enable();
            
            zoomActionReference.action.Enable();
            zoomActionReference.action.performed += Zoom;
        }

        private void OnDisable() {
            switchToLeftCannonActionReference.action.Disable();
            switchToLeftCannonActionReference.action.performed -= SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Disable();
            switchToRightCannonActionReference.action.performed -= SwitchToCannonRight;
            
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= ShowLaser;

            rotateCannonActionReference.action.Disable();
            
            zoomActionReference.action.Disable();
            zoomActionReference.action.performed -= Zoom;
        }

        private void Update() {
            ObjectsReference.Instance.cannonsManager.activeCannon.Rotate(
                rotateCannonActionReference.action.ReadValue<Vector2>().x,
                rotateCannonActionReference.action.ReadValue<Vector2>().y
            );
        }

        private void SwitchToCannonLeft(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.SwitchToLeftCannon();   
        }

        private void SwitchToCannonRight(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.SwitchToRightCannon();
        }
        
        private void ShowLaser(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.ShowLaserOnActivatedRegion();
        }

        private void HideLaser(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.HideLaserOnActivatedRegion();
        }

        private void Zoom(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.ZoomCamera(context.ReadValue<float>());
        }
    }
}