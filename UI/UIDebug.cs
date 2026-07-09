using UnityEngine;

namespace UI {
    public class UIDebug : MonoBehaviour {
        public void ShowHideDebugPanel() {
            if (ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.DEBUG_PANEL].alpha == 0) {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME_MENU);

                ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
                ObjectsReference.Instance.playerController.canMove = false;
                
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.DEBUG_PANEL, true);
            }

            else {
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

                ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();
                ObjectsReference.Instance.playerController.canMove = true;

                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.DEBUG_PANEL, false);
            }
        }
    }
}