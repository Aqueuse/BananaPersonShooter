using InGame.Monkeys.Chimployees;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoBehaviour {
        public Blueprinter blueprinter;

        [SerializeField] private GenericDictionary<CommandRoomPanelType, Transform> cameraTransformByPanelType;
        [SerializeField] private GenericDictionary<CommandRoomPanelType, Transform> cameraFocusByPanelType;

        public ChimployeeCommandRoom chimployeeCommandRoom;
        public Transform chairLifeSimulatorTransform;
        
        public void FocusPanel(int panelTypeEnum) {
            CommandRoomPanelType commandRoomPanelType = (CommandRoomPanelType)panelTypeEnum;
            
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.BANANAMAN_CONTROL) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.GESTION_VIEW) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_GESTION, false);
            }
            
            ObjectsReference.Instance.bananaMan.SetToNotPlayable();
            
            ObjectsReference.Instance.uiInGameVirtualCamera.transform.position = cameraTransformByPanelType[commandRoomPanelType].transform.position;
            ObjectsReference.Instance.uiInGameVirtualCamera.transform.rotation = cameraFocusByPanelType[commandRoomPanelType].transform.rotation;
            ObjectsReference.Instance.uiInGameVirtualCamera.enabled = true;
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.COMMAND_ROOM_PANEL);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.COMMAND_ROOM_PANEL;
            
            if (commandRoomPanelType == CommandRoomPanelType.SPACE_TRAFFIC_CONTROL) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.CANNONS);
                ObjectsReference.Instance.cannonsManager.SwitchToLastCannon();
            }

            else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
        }
        
        public void UnfocusPanel(bool isOnUI) {
            ObjectsReference.Instance.uiInGameVirtualCamera.enabled = false;
            ObjectsReference.Instance.cameraPlayer.ActivateCamera();
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (ObjectsReference.Instance.bananaGun.bananaGunGameObject.activeInHierarchy)
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            if (!isOnUI) {
                ObjectsReference.Instance.gameManager.gameContext = GameContext.BANANAMAN_CONTROL;
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            }
        }
    }
}
