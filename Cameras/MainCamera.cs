using Cinemachine;
using Settings;
using UnityEngine;

namespace Cameras {
    enum CameraStyle {
        TPS,
        SHOOT
    }

    public class MainCamera : MonoSingleton<MainCamera> {
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        [SerializeField] private CinemachineVirtualCamera dialogueCamera;
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

        public void SwitchToDialogueCamera() {
            dialogueCamera.Priority = 14;
        }

        public void SwitchToFreeLookCamera() {
            dialogueCamera.Priority = 3;
        }

        public void Return_back_To_Player() {
            cinemachineFreeLook.ForceCameraPosition (
                new Vector3(0.0053f,-5.9258f,-0.2389f),
                new Quaternion(0.00127450912f,-0.997444868f,0.0690134689f,-0.0184203554f)
            );
        }

        public void Set0Sensibility() {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
        }

        public void SetNormalSensibility() {
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = GameSettings.Instance.lookSensibility;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = GameSettings.Instance.lookSensibility * 400;
        }
    }
}
