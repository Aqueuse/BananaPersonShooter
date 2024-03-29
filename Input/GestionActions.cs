﻿using Cameras;
using InGame.Items.ItemsProperties;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public enum MouseMode {
        NONE,
        ROTATE,
        MOVE
    }
    
    public class GestionActions : InputActions {
        private float _counter;

        private CameraGestion _gestionCamera;

        private ItemScriptableObject selectedBuildableScriptableObject;

        [SerializeField] private InputActionReference buildInputActionReference;
        [SerializeField] private InputActionReference harvestInputActionReference;
        public InputActionReference repairActionReference;

        [SerializeField] private InputActionReference rotateInputActionReference;
        
        [SerializeField] private InputActionReference moveCameraWithMouseInputActionReference;
        [SerializeField] private InputActionReference rotateCameraInputActionReference;

        [SerializeField] private InputActionReference moveCameraKeyboardInputActionReference;
        [SerializeField] private InputActionReference moveUpDownCameraInputActionReference;
        
        [SerializeField] private InputActionReference mouseModeRotateInputActionReference;
        [SerializeField] private InputActionReference mouseModeMoveInputActionReference;
        
        public MouseMode mouseMode = MouseMode.NONE;

        [SerializeField] private float mouseMoveSensibility = 0.5f; 
        
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

            harvestInputActionReference.action.Enable();
            harvestInputActionReference.action.performed += Harvest;

            repairActionReference.action.Enable();
            repairActionReference.action.performed += Repair;
            
            rotateInputActionReference.action.Enable();
            rotateInputActionReference.action.performed += Rotate;
            
            moveCameraKeyboardInputActionReference.action.Enable();
            moveCameraKeyboardInputActionReference.action.performed += MoveCameraWithKeyboard;
            moveCameraKeyboardInputActionReference.action.canceled += CancelMoveCamera;

            moveUpDownCameraInputActionReference.action.Enable();
            moveUpDownCameraInputActionReference.action.performed += MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled += CancelMoveUpDownCamera;
            
            mouseModeMoveInputActionReference.action.Enable();
            mouseModeMoveInputActionReference.action.performed += SwitchToMoveMode;
            mouseModeMoveInputActionReference.action.canceled += SwitchToNormalMode;

            mouseModeRotateInputActionReference.action.Enable();
            mouseModeRotateInputActionReference.action.performed += SwitchToRotateMode;
            mouseModeRotateInputActionReference.action.canceled += SwitchToNormalMode;
        }

        private void OnDisable() {
            buildInputActionReference.action.Disable();
            buildInputActionReference.action.performed -= Build;

            harvestInputActionReference.action.Disable();
            harvestInputActionReference.action.performed -= Harvest;
            
            repairActionReference.action.Disable();
            repairActionReference.action.performed -= Repair;
            
            rotateInputActionReference.action.Disable();
            rotateInputActionReference.action.performed -= Rotate;

            moveCameraKeyboardInputActionReference.action.Disable();
            moveCameraKeyboardInputActionReference.action.performed -= MoveCameraWithKeyboard;
            moveCameraKeyboardInputActionReference.action.canceled -= CancelMoveCamera;

            moveUpDownCameraInputActionReference.action.Disable();
            moveUpDownCameraInputActionReference.action.performed -= MoveUpDownCamera;
            moveUpDownCameraInputActionReference.action.canceled -= CancelMoveUpDownCamera;
            
            mouseModeMoveInputActionReference.action.Disable();
            mouseModeMoveInputActionReference.action.performed -= SwitchToMoveMode;
            mouseModeMoveInputActionReference.action.canceled -= SwitchToNormalMode; 

            mouseModeRotateInputActionReference.action.Disable();
            mouseModeRotateInputActionReference.action.performed -= SwitchToRotateMode;
            mouseModeRotateInputActionReference.action.canceled -= SwitchToNormalMode;
        }
        
        private void Rotate(InputAction.CallbackContext context) {
            var contextValue = context.ReadValue<float>(); 
            
            if (contextValue < 0) {
                ObjectsReference.Instance.gestionBuild.RotateGhost(Vector3.up);
            }

            if (contextValue > 0) {
                ObjectsReference.Instance.gestionBuild.RotateGhost(Vector3.down);
            }
        }
        
        private void Build(InputAction.CallbackContext context) {
            ObjectsReference.Instance.gestionBuild.ValidateBuildable();
        }
        
        private void Harvest(InputAction.CallbackContext context) {
            ObjectsReference.Instance.scan.harvest();
        }
        
        private void Repair(InputAction.CallbackContext context) {
            ObjectsReference.Instance.gestionBuild.RepairBuildable();
        }
        
        /// /// CAMERA
        private void MoveCameraWithMouse(InputAction.CallbackContext context) {
            if (mouseMode != MouseMode.MOVE) return;
            
            _gestionCamera.Move(context.ReadValue<Vector2>().y * mouseMoveSensibility, -context.ReadValue<Vector2>().x * mouseMoveSensibility);
        }
        
        private void MoveCameraWithKeyboard(InputAction.CallbackContext context) {
            moveCameraWithMouseInputActionReference.action.Disable();
            moveCameraWithMouseInputActionReference.action.performed -= MoveCameraWithMouse;
            moveCameraWithMouseInputActionReference.action.canceled -= CancelMoveCamera;
            
            rotateCameraInputActionReference.action.Disable();
            
            _gestionCamera.Move(context.ReadValue<Vector2>().y, -context.ReadValue<Vector2>().x);
        }
        
        private void CancelMoveCamera(InputAction.CallbackContext context) {
            _gestionCamera.CancelMove();
            
            moveCameraWithMouseInputActionReference.action.Enable();
            moveCameraWithMouseInputActionReference.action.performed += MoveCameraWithMouse;
            moveCameraWithMouseInputActionReference.action.canceled += CancelMoveCamera;

            rotateCameraInputActionReference.action.Enable();
        }

        private void CancelMoveUpDownCamera(InputAction.CallbackContext context) {
            _gestionCamera.CancelMoveUpDown();
        }

        private void MoveUpDownCamera(InputAction.CallbackContext context) {
            _gestionCamera.MoveUpDown(context.ReadValue<Vector2>().y);
        }
        
        private void RotateCamera(InputAction.CallbackContext context) {
            _gestionCamera.Rotate(context.ReadValue<Vector2>());
        }

        private void CancelRotateCamera(InputAction.CallbackContext context) {
        }
        
        private void SwitchToRotateMode(InputAction.CallbackContext context) {
            mouseMode = MouseMode.ROTATE;
            
            moveCameraWithMouseInputActionReference.action.Disable();
            moveCameraWithMouseInputActionReference.action.performed -= MoveCameraWithMouse;
            moveCameraWithMouseInputActionReference.action.canceled -= CancelMoveCamera;
            
            rotateCameraInputActionReference.action.Enable();
            rotateCameraInputActionReference.action.performed += RotateCamera;
            rotateCameraInputActionReference.action.canceled += CancelRotateCamera;
        }

        private void SwitchToMoveMode(InputAction.CallbackContext context) {
            mouseMode = MouseMode.MOVE;
            
            moveCameraWithMouseInputActionReference.action.Enable();
            moveCameraWithMouseInputActionReference.action.performed += MoveCameraWithMouse;
            moveCameraWithMouseInputActionReference.action.canceled += CancelMoveCamera;
            
            rotateCameraInputActionReference.action.Disable();
            rotateCameraInputActionReference.action.performed -= RotateCamera;
            rotateCameraInputActionReference.action.canceled -= CancelRotateCamera;
        }
        
        private void SwitchToNormalMode(InputAction.CallbackContext context) {
            moveCameraWithMouseInputActionReference.action.Disable();
            moveCameraWithMouseInputActionReference.action.performed -= MoveCameraWithMouse;
            moveCameraWithMouseInputActionReference.action.canceled -= CancelMoveCamera;

            rotateCameraInputActionReference.action.Disable();
            rotateCameraInputActionReference.action.performed -= RotateCamera;
            rotateCameraInputActionReference.action.canceled -= CancelRotateCamera;
            
            mouseMode = MouseMode.NONE;
        }
    }
}
