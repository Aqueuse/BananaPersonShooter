using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    public class VisitorsWaitingListActions : MonoBehaviour {
        [SerializeField] private InputActionReference quitInputActionReference;
    
        private void OnEnable() {
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
}
