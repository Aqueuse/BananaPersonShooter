using Cinemachine;
using UnityEngine;

namespace Cameras {
    public class MainCamera : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook cinemachineFreeLook;

       private CinemachineCameraOffset _cinemachineCameraOffset;
       
        private void Start() {
            _cinemachineCameraOffset = cinemachineFreeLook.GetComponentInChildren<CinemachineCameraOffset>();
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
            if (_cinemachineCameraOffset.m_Offset.z > 1.73f) return;
            
            if (_cinemachineCameraOffset.m_Offset.z <= 1.73f) {
                _cinemachineCameraOffset.m_Offset.z += 0.1f;
            }
        }
        
        public void DezoomCamera() {
            if (_cinemachineCameraOffset.m_Offset.z < 0f) return;
            
            if (_cinemachineCameraOffset.m_Offset.z >= 0f) {
                _cinemachineCameraOffset.m_Offset.z -= 0.1f;
            }
        }

        public void AddToFOV(float acceleration) {
            //_cinemachineFollowZoom.m_Width = acceleration;
        }
    }
}
