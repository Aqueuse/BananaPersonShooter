using System;
using Cinemachine;
using UI.InGame.CommandRoomControlPanels;
using UnityEngine;

namespace InGame.CommandRoomPanelControls {
    public class GestionPanel : MonoBehaviour {
        [SerializeField] private CinemachineVirtualCamera gestionVirtualCamera;
        [SerializeField] private CanvasGroup gestionCanvasGroup;
        [SerializeField] private CanvasGroup dataNotAvailablePanelCanvasGroup;

        [SerializeField] private UIgestionPanel _uIgestionPanel;
        private RegionType _regionType;

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
            _regionType = (RegionType)Enum.Parse(typeof(RegionType), sceneTypeString);
            _uIgestionPanel.ShowMapCalque(_regionType);

            if (_regionType != RegionType.COROLLE && _regionType != RegionType.MAP01) {
                ShowDataNotAvailablePanel();
                return;
            }
            
            dataNotAvailablePanelCanvasGroup.alpha = 0;
            
            _uIgestionPanel.RefreshWorldDataUI();
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
