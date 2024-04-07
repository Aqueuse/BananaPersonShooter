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
        
        public void FocusPanel(CommandRoomPanelType commandRoomPanelType) {
            commandRoomVirtualCamera.Follow = cameraTransformByPanelType[commandRoomPanelType].transform;
            commandRoomVirtualCamera.LookAt = cameraFocusByPanelType[commandRoomPanelType].transform;
            commandRoomVirtualCamera.Priority = 200;
            
            ObjectsReference.Instance.audioManager.PlayEffect(SoundEffectType.BUTTON_INTERACTION, 0);

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;

            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_COMMAND_ROOM_PANEL;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.UI);
            
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.playerController.canMove = false;
        }

        public void UnfocusPanel() {
            commandRoomVirtualCamera.Priority = 0;
            
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_GAME;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);
        }
    }
}
