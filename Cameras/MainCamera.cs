using Cinemachine;
using UnityEngine;

namespace Cameras {
    public enum CameraModeType {
        PLAYER_VIEW,
        TOP_DOWN_VIEW
    }
    
    public class MainCamera : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook bananaManCamera;
        [SerializeField] private CinemachineVirtualCamera topDownCamera;

        [SerializeField] private Material foliageMaterial;

        public CameraGestion cameraGestion;
        private CinemachineCameraOffset _bananaManCameraOffset;

        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        public CameraModeType cameraModeType;

        private void Start() {
            cameraGestion = ObjectsReference.Instance.gestionCamera;
        }

        public void AddToFOV(float acceleration) {
            //_cinemachineFollowZoom.m_Width = acceleration;
        }

        private void Switch_To_Camera_View(CameraModeType cameraModeType) {
            this.cameraModeType = cameraModeType;

            switch (cameraModeType) {
                case CameraModeType.PLAYER_VIEW:
                    bananaManCamera.m_Priority = 20;
                    topDownCamera.m_Priority = 5;

                    foliageMaterial.SetFloat(Alpha, 1);

                    cameraGestion.enabled = false;
                    break;
                case CameraModeType.TOP_DOWN_VIEW:
                    bananaManCamera.m_Priority = 5;
                    topDownCamera.m_Priority = 20;

                    cameraGestion.enabled = true;
                    cameraGestion.ResetPosition();
                    
                    foliageMaterial.SetFloat(Alpha, 0);
                    break;
            }
        }
        
        public void SwitchToGestionView() {
            ObjectsReference.Instance.gameManager.gameContext = GameContext.IN_MINICHIMP_VIEW;

            ObjectsReference.Instance.uiDescriptionsManager.HideAllPanels();
            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, true);
            ObjectsReference.Instance.uiBlueprintsInventory.RefreshUInventory();
            ObjectsReference.Instance.uInventoriesManager.FocusInventory();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BUILDABLES_INVENTORY, true);

            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 0;
            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();

            ObjectsReference.Instance.gestionCamera.enabled = true;

            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CameraModeType.TOP_DOWN_VIEW);
            ObjectsReference.Instance.gestionMode.enabled = true;
            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.BANANAGUN_UI);
        }
        
        public void CloseGestionView() {
            ObjectsReference.Instance.uInventoriesManager.GetCurrentUIHelper().ShowDefaultHelper();

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.INVENTORIES, false);
            ObjectsReference.Instance.uiDescriptionsManager.HideAllPanels();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            ObjectsReference.Instance.uiManager.SetActive(UICanvasGroupType.BUILDABLES_INVENTORY, false);
            ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.CROSSHAIRS].alpha = 1;

            ObjectsReference.Instance.build.CancelGhost();
            ObjectsReference.Instance.gestionMode.enabled = false;

            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CameraModeType.PLAYER_VIEW);

            ObjectsReference.Instance.cameraPlayer.SetNormalSensibility();

            ObjectsReference.Instance.inputManager.SwitchContext(InputContext.GAME);

            ObjectsReference.Instance.uiDescriptionsManager.HideAllPanels();
        }
    }
}
