using Cinemachine;
using InGame.Monkeys.Chimployees;
using InGame.Monkeys.Minichimps;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class CommandRoomControlPanelsManager : MonoBehaviour {
        [SerializeField] private GenericDictionary<CommandRoomPanelType, ManageAccess> manageAccessesByPanelType;

        public Assembler assembler;

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

        private void FocusPanel(CommandRoomPanelType commandRoomPanelType) {
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

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_COMMAND_ROOM_PANEL;
            
            ObjectsReference.Instance.bananaGunActionsSwitch.gameObject.SetActive(false);
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);
        }
        
        public void FocusPanel(int panelTypeEnum) {
            FocusPanel((CommandRoomPanelType)panelTypeEnum);
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
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
