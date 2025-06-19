using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunBuildingActions : InputActions {
        [SerializeField] private InputActionReference buildActionReference;
        
        [SerializeField] private InputActionReference rotateGhostActionReference;
        [SerializeField] private InputActionReference moveAwayCloserGhostActionReference;

        [SerializeField] private BuildAction buildAction;

        private void OnEnable() {
            buildActionReference.action.Enable();
            buildActionReference.action.performed += Build;
            
            rotateGhostActionReference.action.Enable();
            rotateGhostActionReference.action.performed += RotateGhost;

            moveAwayCloserGhostActionReference.action.Enable();
            moveAwayCloserGhostActionReference.action.performed += MoveAwayCloserGhostTarget;
        }

        private void OnDisable() {
            buildActionReference.action.Disable();
            buildActionReference.action.performed -= Build;
            
            rotateGhostActionReference.action.Disable();
            rotateGhostActionReference.action.performed -= RotateGhost;
            
            moveAwayCloserGhostActionReference.action.Disable();
            moveAwayCloserGhostActionReference.action.performed -= MoveAwayCloserGhostTarget;
        }
        
        private void RotateGhost(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.bananaGunMode != BananaGunMode.BUILD) return;
            
            var contextValue = context.ReadValue<Vector2>(); 
            
            if (contextValue.x < 0) {
                buildAction.RotateGhost(Vector3.up);
            }

            if (contextValue.x > 0) {
                buildAction.RotateGhost(Vector3.down);
            }
            
            if (ObjectsReference.Instance.bottomSlots.GetSelectedSlot().buildableType != BuildableType.BUMPER) return;

            if (contextValue.y < 0) {
                buildAction.RotateGhost(Vector3.left);
            }

            if (contextValue.y > 0) {
                buildAction.RotateGhost(Vector3.right);
            }
        }

        private void MoveAwayCloserGhostTarget(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.bananaGunMode == BananaGunMode.BUILD) return;

            var placementLocalPosition = buildAction.buildablePlacementTransform.localPosition;

            if (context.ReadValue<Vector2>().y > 1) {
                if (placementLocalPosition.z < 25) {
                    placementLocalPosition.z += 1f;
                    buildAction.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }

            if (context.ReadValue<Vector2>().y < 1) {
                if (placementLocalPosition.z > 4) {
                    placementLocalPosition.z -= 1f;
                    buildAction.buildablePlacementTransform.localPosition = placementLocalPosition;
                }
            }
        }

        private void Build(InputAction.CallbackContext callbackContext) {
            buildAction.PlaceBlueprint();
            buildAction.HideGhost();
        }
    }
}