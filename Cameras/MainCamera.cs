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

        public CameraGestionDragRotate cameraGestionDrag;
        public CameraGestionRelativeMove cameraGestionRelativeMove;
        
        private CinemachineCameraOffset _bananaManCameraOffset;

        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        public CameraModeType cameraModeType;

        private void Start() {
            cameraGestionDrag = ObjectsReference.Instance.gestionDragCamera;
            cameraGestionRelativeMove = ObjectsReference.Instance.gestionRelativeMoveCamera;
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

                    cameraGestionDrag.enabled = false;

                    foliageMaterial.SetFloat(Alpha, 1);
                    break;
                case CameraModeType.TOP_DOWN_VIEW:
                    bananaManCamera.m_Priority = 5;
                    topDownCamera.m_Priority = 20;

                    cameraGestionDrag.enabled = true;
                    cameraGestionDrag.ResetPosition();

                    foliageMaterial.SetFloat(Alpha, 0);
                    break;
            }
        }
        
        public void SwitchToGestionView() {
            cameraGestionDrag.enabled = true;
            cameraGestionRelativeMove.enabled = true;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CameraModeType.TOP_DOWN_VIEW);
        }
        
        public void SwitchToBananaManView() {
            cameraGestionDrag.enabled = false;
            cameraGestionRelativeMove.enabled = false;

            ObjectsReference.Instance.mainCamera.Switch_To_Camera_View(CameraModeType.PLAYER_VIEW);
        }
    }
}
