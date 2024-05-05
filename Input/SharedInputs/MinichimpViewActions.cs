using Cameras;
using InGame.Items.ItemsProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MinichimpViewActions : InputActions {
        [SerializeField] private InputActionReference buildInputActionReference;
        [SerializeField] private InputActionReference harvestOrRepairInputActionReference;
        [SerializeField] private InputActionReference moveUpDownCameraInputActionReference;
        
        [SerializeField] private InputActionReference rotateGhostInputActionReference;
        
        [SerializeField] private InputActionReference cancelInputActionReference;
        
        [SerializeField] private float mouseMoveSensibility = 0.5f; 

        private float _counter;

        private CameraGestion _gestionCamera;

        private ItemScriptableObject selectedBuildableScriptableObject;
        
        private void Start() {
            _gestionCamera = ObjectsReference.Instance.gestionCamera;
        }
    
        private void OnEnable() {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
                    
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.playerController.canMove = false;
            ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
            
            buildInputActionReference.action.Enable();
            buildInputActionReference.action.performed += Build;

            harvestOrRepairInputActionReference.action.Enable();
            harvestOrRepairInputActionReference.action.performed += HarvestOrRepair;
            
            rotateGhostInputActionReference.action.Enable();
            rotateGhostInputActionReference.action.performed += RotateGhost;
            
            moveUpDownCameraInputActionReference.action.Enable();
            moveUpDownCameraInputActionReference.action.performed += MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled += CancelMoveUpDownCamera;
            
            cancelInputActionReference.action.Enable();
            cancelInputActionReference.action.performed += Cancel;
        }

        private void OnDisable() {
            buildInputActionReference.action.Disable();
            buildInputActionReference.action.performed -= Build;

            harvestOrRepairInputActionReference.action.Disable();
            harvestOrRepairInputActionReference.action.performed -= HarvestOrRepair;

            rotateGhostInputActionReference.action.Disable();
            rotateGhostInputActionReference.action.performed -= RotateGhost;

            moveUpDownCameraInputActionReference.action.Disable();
            moveUpDownCameraInputActionReference.action.performed -= MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled -= CancelMoveUpDownCamera;

            cancelInputActionReference.action.Disable();
            cancelInputActionReference.action.performed -= Cancel;
        }

        private void MoveUpDownCamera(InputAction.CallbackContext context) {
            _gestionCamera.MoveUpDown(context.ReadValue<Vector2>().y);
        }

        private void RotateGhost(InputAction.CallbackContext context) {
            var contextValue = context.ReadValue<float>(); 
            
            if (contextValue < 0) {
                ObjectsReference.Instance.gestionMode.RotateGhost(Vector3.up);
            }

            if (contextValue > 0) {
                ObjectsReference.Instance.gestionMode.RotateGhost(Vector3.down);
            }
        }
        
        private void Build(InputAction.CallbackContext context) {
            ObjectsReference.Instance.gestionMode.ValidateBuildable();
        }
        
        private void HarvestOrRepair(InputAction.CallbackContext context) {
            ObjectsReference.Instance.scan.RepairOrHarvest();
        }
        
        private void CancelMoveUpDownCamera(InputAction.CallbackContext context) {
            _gestionCamera.CancelMoveUpDown();
        }
        
        private void Cancel(InputAction.CallbackContext context) {
            // cancel build
            // or switch back to game
        }
    }
}