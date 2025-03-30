using Cinemachine;
using InGame.Monkeys.Chimployees;
using InGame.Monkeys.Minichimps;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<CommandRoomPanelType, ManageAccess> manageAccessesByPanelType;

        public Blueprinter blueprinter;

        public CinemachineVirtualCamera commandRoomVirtualCamera;
        [SerializeField] private GenericDictionary<CommandRoomPanelType, Transform> cameraTransformByPanelType;
        [SerializeField] private GenericDictionary<CommandRoomPanelType, Transform> cameraFocusByPanelType;

        public MiniChimp miniChimp;
        public ChimployeeCommandRoom chimployeeCommandRoom;
        public Transform chairLifeSimulatorTransform;
        
        public void Init() {
            if (ObjectsReference.Instance.bananaMan.tutorialFinished) {
                foreach (var manageAccess in manageAccessesByPanelType) {
                    manageAccess.Value.AuthorizeUsage();
                }
            }
            else {
                foreach (var manageAccess in manageAccessesByPanelType) {
                    manageAccess.Value.ForbidUsage();
                }
            }
        }
        
        public void FocusPanel(int panelTypeEnum) {
            CommandRoomPanelType commandRoomPanelType = (CommandRoomPanelType)panelTypeEnum;
            
            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GAME) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, false);
            }

            if (ObjectsReference.Instance.gameManager.gameContext == GameContext.IN_GESTION_VIEW) {
                ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_GESTION, false);
            }

            commandRoomVirtualCamera.Follow = cameraTransformByPanelType[commandRoomPanelType].transform;
            commandRoomVirtualCamera.LookAt = cameraFocusByPanelType[commandRoomPanelType].transform;
            commandRoomVirtualCamera.Priority = 200;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
            
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_COMMAND_ROOM_PANEL;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;

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
            commandRoomVirtualCamera.Priority = 0;

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.HUD_BANANAMAN, true);

            if (!isOnUI) {
                ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
                ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
            }
        }
    }
}
