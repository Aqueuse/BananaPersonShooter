using InGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapActions : InputActions {
    [SerializeField] private InputActionReference moveActionReference;
    
    [SerializeField] private InputActionReference startDraggingActionReference;
    [SerializeField] private InputActionReference dragActionReference;

    [SerializeField] private InputActionReference zoomDezoomReference;
    [SerializeField] private InputActionReference switchToGameReference;
    
    private Map map;

    private bool isDragging;
    
    private void Start() {
        map = ObjectsReference.Instance.map;
    }

    private void OnEnable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
        
        ObjectsReference.Instance.playerController.canMove = false;
        ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
        
        moveActionReference.action.Enable();
        moveActionReference.action.performed += Move;
        
        zoomDezoomReference.action.Enable();
        zoomDezoomReference.action.performed += ZoomDezoom;
        
        switchToGameReference.action.Enable();
        switchToGameReference.action.performed += SwitchBackToGame;
        
        startDraggingActionReference.action.Enable();
        startDraggingActionReference.action.performed += StartDragging;
    }

    private void OnDisable() {
        moveActionReference.action.Disable();
        moveActionReference.action.performed -= Move;
        
        zoomDezoomReference.action.Disable();
        zoomDezoomReference.action.performed -= ZoomDezoom;
        
        switchToGameReference.action.Disable();
        switchToGameReference.action.performed -= SwitchBackToGame;
        
        startDraggingActionReference.action.Disable();
        startDraggingActionReference.action.performed -= StartDragging;
    }

    private void Move(InputAction.CallbackContext callbackContext) {
        // move the space cam
        var movement2D = callbackContext.ReadValue<Vector2>();
        var movement = new Vector3(-movement2D.x, 0, -movement2D.y); 
        
        map.Move(movement);
    }

    private void StartDragging(InputAction.CallbackContext callbackContext) {
        if (callbackContext.performed && !isDragging) {
            dragActionReference.action.Enable();
            dragActionReference.action.performed += Drag;

            isDragging = true;
            
            return;
        }

        if (callbackContext.performed && isDragging) {
            dragActionReference.action.Disable();
            dragActionReference.action.performed -= Drag;

            isDragging = false;
        }
    }
    
    private void Drag(InputAction.CallbackContext callbackContext) {
        var movement2D = callbackContext.ReadValue<Vector2>();
        var movement = new Vector3(-movement2D.x, 0, -movement2D.y); 
        
        map.Move(movement);
    }

    private void ZoomDezoom(InputAction.CallbackContext callbackContext) {
        // zoom dezoom the space cam
        // with a max / min value
        if (callbackContext.ReadValue<float>() < 0) {
            map.Zoom();
        }

        else {
            map.Dezoom();
        }
    }

    private void SwitchBackToGame(InputAction.CallbackContext callbackContext) {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
        
        ObjectsReference.Instance.playerController.canMove = true;
        ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
        
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        
        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAP, false);
    }
}
