using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class CannonsActions : InputActions {
        [SerializeField] private InputActionReference switchToLeftCannonActionReference;
        [SerializeField] private InputActionReference switchToRightCannonActionReference;

        [SerializeField] private InputActionReference shootActionReference;
        [SerializeField] private InputActionReference rotateCannonActionReference;
        
        [SerializeField] private InputActionReference aspireActionReference;

        [SerializeField] private InputActionReference zoomActionReference;

        [SerializeField] private InputActionReference escapeCannonActionReference;

        private void OnEnable() {
            switchToLeftCannonActionReference.action.Enable();
            switchToLeftCannonActionReference.action.performed += SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Enable();
            switchToRightCannonActionReference.action.performed += SwitchToCannonRight;
            
            shootActionReference.action.Enable();
            shootActionReference.action.performed += ShootLaser;

            aspireActionReference.action.Enable();
            aspireActionReference.action.performed += AspireDebris;
            aspireActionReference.action.canceled += StopAspireDebris;

            rotateCannonActionReference.action.Enable();
            
            zoomActionReference.action.Enable();
            zoomActionReference.action.performed += Zoom;
            
            escapeCannonActionReference.action.Enable();
            escapeCannonActionReference.action.performed += EscapeCannon;
        }

        private void OnDisable() {
            switchToLeftCannonActionReference.action.Disable();
            switchToLeftCannonActionReference.action.performed -= SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Disable();
            switchToRightCannonActionReference.action.performed -= SwitchToCannonRight;
            
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= ShootLaser;

            aspireActionReference.action.Disable();
            aspireActionReference.action.performed -= AspireDebris;
            aspireActionReference.action.canceled -= StopAspireDebris;

            rotateCannonActionReference.action.Disable();
            
            zoomActionReference.action.Disable();
            zoomActionReference.action.performed -= Zoom;
            
            escapeCannonActionReference.action.Disable();
            escapeCannonActionReference.action.performed -= EscapeCannon;
        }

        private void Update() {
            ObjectsReference.Instance.cannonsManager.activeCannon.Rotate(
                rotateCannonActionReference.action.ReadValue<Vector2>().x * ObjectsReference.Instance.gameSettings.horizontalLookSensibility / 5,
                rotateCannonActionReference.action.ReadValue<Vector2>().y * ObjectsReference.Instance.gameSettings.verticalLookSensibility / 5
            ); 
        }

        private void SwitchToCannonLeft(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.SwitchToLeftCannon();   
        }

        private void SwitchToCannonRight(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.SwitchToRightCannon();
        }
        
        private void ShootLaser(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.ShootLaserOnActivatedRegion();
        }

        private void AspireDebris(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.AspireDebris();
        }
        
        private void StopAspireDebris(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.StopAspireDebris();
        }

        private void Zoom(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManager.ZoomCamera(context.ReadValue<float>());
        }
        
        private void EscapeCannon(InputAction.CallbackContext context) {
            ObjectsReference.Instance.commandRoomControlPanelsManager.UnfocusPanel(false);
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}