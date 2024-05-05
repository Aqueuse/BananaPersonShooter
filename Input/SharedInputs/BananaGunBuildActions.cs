using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunBuildActions : InputActions {
        public InputActionReference grabBananaGunActionReference;
        public InputActionReference rotateGhostActionReference;
        public InputActionReference moveAwayCloserGhostActionReference;
        [SerializeField] private InputActionReference confirmBuild;
    
        [SerializeField] private Build build;

        private void OnEnable() {
            grabBananaGunActionReference.action.performed += GrabBananaGun;
            grabBananaGunActionReference.action.canceled += UngrabBananaGun;
            grabBananaGunActionReference.action.Enable();
        
            rotateGhostActionReference.action.Enable();
            rotateGhostActionReference.action.performed += RotateGhost;

            moveAwayCloserGhostActionReference.action.Enable();
            moveAwayCloserGhostActionReference.action.performed += MoveAwayCloserGhostTarget;
        
            confirmBuild.action.Enable();
            confirmBuild.action.performed += ConfirmBuild;
        }

        private void OnDisable() {
            grabBananaGunActionReference.action.performed -= GrabBananaGun;
            grabBananaGunActionReference.action.canceled -= UngrabBananaGun;
            grabBananaGunActionReference.action.Disable();

            rotateGhostActionReference.action.Disable();
            rotateGhostActionReference.action.performed -= RotateGhost;

            moveAwayCloserGhostActionReference.action.Disable();
            moveAwayCloserGhostActionReference.action.performed -= MoveAwayCloserGhostTarget;
        
            confirmBuild.action.Disable();
            confirmBuild.action.performed -= ConfirmBuild;
        }

        private void GrabBananaGun(InputAction.CallbackContext context) {
            build.enabled = true;

            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowBuildHelper();

            ObjectsReference.Instance.bananaGun.GrabBananaGun();
            build.ActivateGhost();
        }
    
        private void UngrabBananaGun(InputAction.CallbackContext context) {
            build.CancelGhost();
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();

            ObjectsReference.Instance.bananaGun.UngrabBananaGun();
            build.CancelGhost();

            build.enabled = false;
        }
    
        private void RotateGhost(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.inputManager.bananaGunActions != BananaGunMode.BUILD) return;

            var contextValue = context.ReadValue<Vector2>(); 

            if (contextValue.x < 0) {
                build.RotateGhost(Vector3.left);
            }

            if (contextValue.x > 0) {
                build.RotateGhost(Vector3.right);
            }

            if (contextValue.y < 0) {
                build.RotateGhost(Vector3.back);
            }

            if (contextValue.y > 0) {
                build.RotateGhost(Vector3.forward);
            }
        }

        private void MoveAwayCloserGhostTarget(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.inputManager.bananaGunActions == BananaGunMode.BUILD) return;

            var placementLocalPosition = build.buildablePlacementTransform.localPosition;

            if (context.ReadValue<Vector2>().y > 1) {
                if (placementLocalPosition.z < 25) {
                    placementLocalPosition.z += 1f;
                    build.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }

            if (context.ReadValue<Vector2>().y < 1) {
                if (placementLocalPosition.z > 4) {
                    placementLocalPosition.z -= 1f;
                    build.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
        }
    
        private void ConfirmBuild(InputAction.CallbackContext callbackContext) {
            build.ValidateBuildable();                    
            build.CancelGhost();
        }
    }
}