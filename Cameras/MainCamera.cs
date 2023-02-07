using Cinemachine;
using UnityEngine;

namespace Cameras {
    enum CameraStyle {
        TPS,
        SHOOT
    }

    public class MainCamera : MonoSingleton<MainCamera> {
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        public GameObject bananaSplashVideo;
        private CameraStyle cameraStyle;

        private float blending;
        private float blendingSpeed;

        private void Start() {
            cameraStyle = CameraStyle.TPS;
            blending = 0;
            blendingSpeed = 1.5f;
        }

        private void Update() {
            if (cameraStyle == CameraStyle.SHOOT && cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>().m_Offset.x < 1) {
                blending += Time.deltaTime * blendingSpeed;
                cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>().m_Offset.x = blending;
            }

            if (cameraStyle == CameraStyle.TPS && cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>().m_Offset.x > 0) {
                blending -= Time.deltaTime * blendingSpeed;
                cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>().m_Offset.x = blending;
            }
        }

        public void Switch_To_TPS_Target() {
            cameraStyle = CameraStyle.TPS;
        }

        public void Switch_To_Shoot_Target() {
            cameraStyle = CameraStyle.SHOOT;
        }
    }
}
