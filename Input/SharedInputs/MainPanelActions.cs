using UnityEngine;
using UnityEngine.InputSystem;

namespace SharedInputs {
    public class MainPanelActions : InputActions {
        [SerializeField] private InputActionReference hideMainPanelActionReference;
        
        private void OnEnable() {
            hideMainPanelActionReference.action.Enable();
            hideMainPanelActionReference.action.performed += HideMainPanel;
        }

        private void OnDisable() {
            hideMainPanelActionReference.action.Disable();
            hideMainPanelActionReference.action.performed -= HideMainPanel;
        }
        
        private void HideMainPanel(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.MAIN_PANEL, false);
            ObjectsReference.Instance.inputManager.SwitchBackToGame();
        }
    }
}