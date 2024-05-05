using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class CannonsPanelActions : InputActions {
        [SerializeField] private InputActionReference switchToLeftCannonActionReference;
        [SerializeField] private InputActionReference switchToRightCannonActionReference;
        
        [SerializeField] private InputActionReference shootActionReference;
        [SerializeField] private InputActionReference rotateCannonActionReference;
        [SerializeField] private InputActionReference quitActionReference;

        private void OnEnable() {
            switchToLeftCannonActionReference.action.Enable();
            switchToLeftCannonActionReference.action.performed += SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Enable();
            switchToRightCannonActionReference.action.performed += SwitchToCannonRight;
            
            shootActionReference.action.Enable();
            shootActionReference.action.performed += Shoot;

            rotateCannonActionReference.action.Enable();
            rotateCannonActionReference.action.performed += RotateCannon;

            quitActionReference.action.Enable();
            quitActionReference.action.performed += Quit;
        }

        private void OnDisable() {
            switchToLeftCannonActionReference.action.Disable();
            switchToLeftCannonActionReference.action.performed -= SwitchToCannonLeft;

            switchToRightCannonActionReference.action.Disable();
            switchToRightCannonActionReference.action.performed -= SwitchToCannonRight;
            
            shootActionReference.action.Disable();
            shootActionReference.action.performed -= Shoot;

            rotateCannonActionReference.action.Enable();
            rotateCannonActionReference.action.performed -= RotateCannon;

            quitActionReference.action.Disable();
            quitActionReference.action.performed -= Quit;
        }

        private void SwitchToCannonLeft(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManagement.SwitchToLeftCannon();   
        }

        private void SwitchToCannonRight(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManagement.SwitchToRightCannon();
        }
        
        private void RotateCannon(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManagement.RotateCannon(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y);
        }

        private void Shoot(InputAction.CallbackContext context) {
            ObjectsReference.Instance.cannonsManagement.ShootOnActivatedRegion();
        }
        
        private void Quit(InputAction.CallbackContext callbackContext) {
            ObjectsReference.Instance.cannonsManagement.StopCannonControl();
        }
    }
}