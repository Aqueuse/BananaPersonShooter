using UnityEngine;
using UnityEngine.InputSystem;

public class BananaSelectorActions : InputActions {
    [SerializeField] private InputActionReference switchBackToGameInputActionReference;
    
    private void OnEnable() {
        switchBackToGameInputActionReference.action.performed += SwitchBackToGame;
        switchBackToGameInputActionReference.action.Enable();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ObjectsReference.Instance.uiActions.enabled = true;

        ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
        ObjectsReference.Instance.playerController.canMove = false;
        ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnDisable() {
        switchBackToGameInputActionReference.action.performed -= SwitchBackToGame;
        switchBackToGameInputActionReference.action.Disable();
    }

    private void SwitchBackToGame(InputAction.CallbackContext callbackContext) {
        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BANANA_SELECTOR, false);

        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

        Time.timeScale = 1f;
    }
}
