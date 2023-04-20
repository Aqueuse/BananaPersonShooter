using Cinemachine;
using UnityEngine;

namespace Cameras {
    enum CameraStyle {
        TPS = 0,
        SHOOT = 1
    }

    public class MainCamera : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;
        [SerializeField] private CinemachineVirtualCamera dialogueCamera;
       private CameraStyle _cameraStyle;

       private CinemachineCameraOffset _cinemachineCameraOffset;

        private float _blending;
        private float _blendingSpeed;

        private void Start() {
            _cinemachineCameraOffset = cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>();
            
            _cameraStyle = CameraStyle.TPS;
            _blending = 0;
            _blendingSpeed = 1.5f;
        }

        private void Update() {
            if (_cameraStyle == CameraStyle.SHOOT && _cinemachineCameraOffset.m_Offset.x < 1) {
                _blending += Time.deltaTime * _blendingSpeed;
                _cinemachineCameraOffset.m_Offset.x = _blending;
            }

            if (_cameraStyle == CameraStyle.TPS && _cinemachineCameraOffset.m_Offset.x > 0) {
                _blending -= Time.deltaTime * _blendingSpeed;
                _cinemachineCameraOffset.m_Offset.x = _blending;
            }
        }

        public void Switch_To_TPS_Target() {
            _cameraStyle = CameraStyle.TPS;
        }

        public void Switch_To_Shoot_Target() {
            _cameraStyle = CameraStyle.SHOOT;
        }

        public void SwitchToDialogueCamera(Transform target) {
            dialogueCamera.Priority = 14;
            dialogueCamera.LookAt = target;
        }

        public void SwitchToFreeLookCamera() {
            dialogueCamera.Priority = 3;
            dialogueCamera.LookAt = null;
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
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = ObjectsReference.Instance.gameSettings.lookSensibility;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = ObjectsReference.Instance.gameSettings.lookSensibility * 400;
        }
        
        public void ZoomCamera() {
            if (_cinemachineCameraOffset.m_Offset.z > 0) return;
            
            if (_cinemachineCameraOffset.m_Offset.z <= 0) {
                _cinemachineCameraOffset.m_Offset.z += 0.1f;
            }
        }
        
        public void DezoomCamera() {
            if (_cinemachineCameraOffset.m_Offset.z < -1.9f) return;
            
            if (_cinemachineCameraOffset.m_Offset.z >= -1.9f) {
                _cinemachineCameraOffset.m_Offset.z -= 0.1f;
            }
        }
    }
}
