using InGame.Player.BananaGunActions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class BananaGunBuildActions : InputActions {
        [SerializeField] private InputActionReference buildActionReference;
        [SerializeField] private InputActionReference unbuildActionReference;
        
        [SerializeField] private InputActionReference rotateGhostActionReference;
        [SerializeField] private InputActionReference moveAwayCloserGhostActionReference;

        [SerializeField] private Build build;

        private void OnEnable() {
            buildActionReference.action.Enable();
            buildActionReference.action.performed += ConfirmBuild;

            unbuildActionReference.action.Enable();
            unbuildActionReference.action.performed += RepairOrHarvest;
            unbuildActionReference.action.canceled += StopRepairOrHarvest;

            rotateGhostActionReference.action.Enable();
            rotateGhostActionReference.action.performed += RotateGhost;

            moveAwayCloserGhostActionReference.action.Enable();
            moveAwayCloserGhostActionReference.action.performed += MoveAwayCloserGhostTarget;
        }

        private void OnDisable() {
            buildActionReference.action.Disable();
            buildActionReference.action.performed -= ConfirmBuild;
            
            unbuildActionReference.action.Disable();
            unbuildActionReference.action.performed -= RepairOrHarvest;
            unbuildActionReference.action.canceled -= StopRepairOrHarvest;
        }
        
        private void RepairOrHarvest(InputAction.CallbackContext context) {
            build.isBuilding = false;
            
            build.RepairOrHarvest();
        }

        private void StopRepairOrHarvest(InputAction.CallbackContext context) {
            build.isBuilding = true;
        }
        
        private void RotateGhost(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.bananaGunMode != BananaGunMode.BUILD) return;
            
            var contextValue = context.ReadValue<Vector2>(); 
            
            if (contextValue.x < 0) {
                build.RotateGhost(Vector3.up);
            }

            if (contextValue.x > 0) {
                build.RotateGhost(Vector3.down);
            }
            
            if (ObjectsReference.Instance.bananaMan.bananaManData.activeBuildable != BuildableType.BUMPER) return;

            if (contextValue.y < 0) {
                build.RotateGhost(Vector3.left);
            }

            if (contextValue.y > 0) {
                build.RotateGhost(Vector3.right);
            }
        }

        private void MoveAwayCloserGhostTarget(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.bananaMan.bananaGunMode == BananaGunMode.BUILD) return;

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
            build.HideGhost();
        }
    }
}