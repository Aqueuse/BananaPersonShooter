using UnityEngine;
using UnityEngine.InputSystem;

public class VisitorsWaitingListActions : InputActions {
    [SerializeField] private InputActionReference quitInputActionReference;
    
    private void OnEnable() {
        ObjectsReference.Instance.uiActions.enabled = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

        ObjectsReference.Instance.playerController.canMove = false;
        ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;

        quitInputActionReference.action.Enable();
        quitInputActionReference.action.performed += Quit;
    }

    private void OnDisable() {
        quitInputActionReference.action.Disable();
        quitInputActionReference.action.performed -= Quit;
    }

    private void Quit(InputAction.CallbackContext context) {
        ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.VISITOR_WAITING_LIST, false);
            
        ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
    }
}