using Cinemachine;
using UnityEngine;

namespace Cameras {
    public class CameraPlayer : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook bananaManCamera;
        private CinemachineCameraOffset _bananaManCameraOffset;

        private void Start() {
            _bananaManCameraOffset = bananaManCamera.GetComponentInChildren<CinemachineCameraOffset>();
        }
        
        public void Return_back_To_Player() {
            bananaManCamera.ForceCameraPosition (
                new Vector3(0.0053f,-5.9258f,-0.2389f),
                new Quaternion(0.00127450912f,-0.997444868f,0.0690134689f,-0.0184203554f)
            );
        }

        public void Set0Sensibility() {
            bananaManCamera.m_YAxis.m_MaxSpeed = 0;
            bananaManCamera.m_XAxis.m_MaxSpeed = 0;
        }

        public void SetNormalSensibility() {
            bananaManCamera.m_YAxis.m_MaxSpeed = ObjectsReference.Instance.gameSettings.lookSensibility;
            bananaManCamera.m_XAxis.m_MaxSpeed = ObjectsReference.Instance.gameSettings.lookSensibility * 400;
        }
        
        public void ZoomCamera() {
            if (_bananaManCameraOffset.m_Offset.z > 1.73f) return;
            
            if (_bananaManCameraOffset.m_Offset.z <= 1.73f) {
                _bananaManCameraOffset.m_Offset.z += 0.1f;
            }
        }
        
        public void DezoomCamera() {
            if (_bananaManCameraOffset.m_Offset.z < 0f) return;
            
            if (_bananaManCameraOffset.m_Offset.z >= 0f) {
                _bananaManCameraOffset.m_Offset.z -= 0.1f;
            }
        }
    }
}
