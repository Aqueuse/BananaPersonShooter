using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class UiActions : MonoBehaviour {
        [SerializeField] private InputActionReference cancelActionReference;
        [SerializeField] private InputActionReference SwitchToLeftOptionPanel;
        [SerializeField] private InputActionReference SwitchToRightOptionPanel;
        [SerializeField] private InputActionReference pointerActionReference;

        public Vector2 pointerPosition;
        
        private void OnEnable() {
            cancelActionReference.action.Enable();
            cancelActionReference.action.performed += Escape;
            
            SwitchToLeftOptionPanel.action.Enable();
            SwitchToLeftOptionPanel.action.performed += Scroll_Left_Options_Tab;
            
            SwitchToRightOptionPanel.action.Enable();
            SwitchToRightOptionPanel.action.performed += Scroll_Right_Options_Tab;

            pointerActionReference.action.Enable();
            pointerActionReference.action.performed += MovePointer;
            pointerActionReference.action.canceled += MovePointer;
        }

        private void OnDisable() {
            cancelActionReference.action.Disable();
            cancelActionReference.action.performed -= Escape;
            
            SwitchToLeftOptionPanel.action.Disable();
            SwitchToLeftOptionPanel.action.performed -= Scroll_Left_Options_Tab;
            
            SwitchToRightOptionPanel.action.Disable();
            SwitchToRightOptionPanel.action.performed -= Scroll_Right_Options_Tab;
            
            pointerActionReference.action.Disable();
            pointerActionReference.action.performed -= MovePointer;
            pointerActionReference.action.canceled -= MovePointer;
        }

        private void Scroll_Left_Options_Tab(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiOptionsMenu.Switch_to_Left_Tab();
        }
        
        private void Scroll_Right_Options_Tab(InputAction.CallbackContext context) {
            ObjectsReference.Instance.uiOptionsMenu.Switch_to_Right_Tab();
        }
        
        private void Escape(InputAction.CallbackContext context) {
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_HOME) {
                ObjectsReference.Instance.uiManager.ShowHomeMenu();
            }
        
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                if (ObjectsReference.Instance.gestionMode.isGestionModeActivated) {
                    ObjectsReference.Instance.gestionMode.CloseGestionMode();
                    return;
                }

                if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.INVENTORIES].alpha > 0) {
                    ObjectsReference.Instance.uiManager.HideInventories();
                    return;
                }
                
                if (ObjectsReference.Instance.uiManager.isOnSubMenus) {
                    ObjectsReference.Instance.uiManager.ShowGameMenu();
                }
                else {
                    ObjectsReference.Instance.uiManager.HideGameMenu();
                    ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
                }
            }
        }

        private void MovePointer(InputAction.CallbackContext context) {
            pointerPosition = context.ReadValue<Vector2>();
        }
    }
}
