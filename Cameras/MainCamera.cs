using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Cameras {
    public enum CAMERA_MODE {
        PLAYER_VIEW,
        TOP_DOWN_VIEW
    }
    
    public class MainCamera : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook bananaManCamera;
        [SerializeField] private CinemachineVirtualCamera topDownCamera;
        
        private UniversalAdditionalCameraData URP_Asset;
        
        public CameraGestion cameraGestion;
        private CinemachineCameraOffset _bananaManCameraOffset;

        [SerializeField] private Material foliageMaterial;
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        private void Start() {
            URP_Asset = GetComponent<UniversalAdditionalCameraData>();
        }

        public void AddToFOV(float acceleration) {
            //_cinemachineFollowZoom.m_Width = acceleration;
        }

        public void Switch_To_Camera_View(CAMERA_MODE cameraMode) {
            switch (cameraMode) {
                case CAMERA_MODE.PLAYER_VIEW:
                    bananaManCamera.m_Priority = 20;
                    topDownCamera.m_Priority = 5;

                    foliageMaterial.SetFloat(Alpha, 1);
                    break;
                case CAMERA_MODE.TOP_DOWN_VIEW:
                    bananaManCamera.m_Priority = 5;
                    topDownCamera.m_Priority = 20;
                    cameraGestion = ObjectsReference.Instance.gestionCamera;
                    cameraGestion.ResetPosition();
                    
                    foliageMaterial.SetFloat(Alpha, 0);
                    break;
            }
        }
        
        public void SetNormalRenderer() {
            URP_Asset.SetRenderer(0);
        }

        public void SetOutlineRenderer() {
            // render objects in the layer BananaGunSelectable with a white outline
            URP_Asset.SetRenderer(1);
        }
    }
}
