using UnityEngine;

namespace Cameras {
    public enum CameraModeType {
        PLAYER_VIEW,
        TOP_DOWN_VIEW,
        HOME_VIEW
    }
    
    public class CamerasManager : MonoBehaviour {
        public Camera bananaManCamera;
        public Camera screenshotCamera;
        public Camera homeCamera;
        [SerializeField] private Camera topDownCamera;

        [SerializeField] private Material foliageMaterial;

        public CameraGestionDragRotate cameraGestionDrag;
        public CameraGestionRelativeMove cameraGestionRelativeMove;
        
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");
        
        private void Start() {
            cameraGestionDrag = ObjectsReference.Instance.gestionDragCamera;
            cameraGestionRelativeMove = ObjectsReference.Instance.gestionRelativeMoveCamera;
        }

        public void AddToFOV(float acceleration) {
           //_cinemachineFollowZoom.m_Width = acceleration;
        }

        public void Switch_To_Camera_View(CameraModeType cameraModeType) {
            switch (cameraModeType) {
                case CameraModeType.PLAYER_VIEW:
                    bananaManCamera.enabled = true;
                    topDownCamera.enabled = false;
                    cameraGestionDrag.enabled = false;
                    homeCamera.enabled = false;

                    foliageMaterial.SetFloat(Alpha, 1);
                    break;
                case CameraModeType.TOP_DOWN_VIEW:
                    bananaManCamera.enabled = false;
                    topDownCamera.enabled = false;
                    homeCamera.enabled = false;

                    cameraGestionDrag.enabled = true;
                    cameraGestionDrag.ResetPosition();

                    foliageMaterial.SetFloat(Alpha, 0);
                    break;
                case CameraModeType.HOME_VIEW:
                    bananaManCamera.enabled = false;
                    topDownCamera.enabled = false;
                    cameraGestionDrag.enabled = false;
                    homeCamera.enabled = true;
                    break;
            }
        }
        
        public void SwitchToGestionView() {
            cameraGestionDrag.enabled = true;
            cameraGestionRelativeMove.enabled = true;

            ObjectsReference.Instance.cameraPlayer.Set0Sensibility();
            ObjectsReference.Instance.camerasManager.Switch_To_Camera_View(CameraModeType.TOP_DOWN_VIEW);
        }
        
        public void SwitchToBananaManView() {
            cameraGestionDrag.enabled = false;
            cameraGestionRelativeMove.enabled = false;

            ObjectsReference.Instance.camerasManager.Switch_To_Camera_View(CameraModeType.PLAYER_VIEW);
        }
    }
}
