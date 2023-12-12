using System;
using Cinemachine;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace Game.CommandRoomPanelControls {
    public class GestionPanel : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera gestionVirtualCamera;
        [SerializeField] private CanvasGroup gestionCanvasGroup;
        [SerializeField] private CanvasGroup dataNotAvailablePanelCanvasGroup;

        [SerializeField] private UIgestionPanel _uIgestionPanel;

        public void SwitchToGestionPanel() {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            gestionCanvasGroup.interactable = true;
            gestionCanvasGroup.blocksRaycasts = true;

            gestionVirtualCamera.m_Priority = 20;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION_COMMAND_ROOM_PANEL);
        }

        private void ShowDataNotAvailablePanel() {
            dataNotAvailablePanelCanvasGroup.alpha = 1;
        }

        public void ShowMapData(string sceneTypeString) {
            if (sceneTypeString != "COROLLE" && sceneTypeString != "MAP01") {
                ShowDataNotAvailablePanel();
                return;
            }

            dataNotAvailablePanelCanvasGroup.alpha = 0;

            SceneType sceneType = (SceneType)Enum.Parse(typeof(SceneType), sceneTypeString);

            var map = ObjectsReference.Instance.gameData.mapBySceneName[sceneType]; 

            _uIgestionPanel.RefreshMapDataUI(
                map.mapPropertiesScriptableObject.piratesQuantity,
                map.mapPropertiesScriptableObject.visitorsQuantity,
                map.mapPropertiesScriptableObject.chimployeesQuantity,
                map.piratesDebrisToSpawn+map.visitorsDebrisToSpawn
            );
        }

        public void SwitchBackToGame() {
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;
            gestionCanvasGroup.interactable = false;
            gestionCanvasGroup.blocksRaycasts = false;

            gestionVirtualCamera.m_Priority = 0;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GESTION_COMMAND_ROOM_PANEL);
        }
    }
}
