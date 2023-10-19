using Cinemachine;
using Enums;
using Input.UIActions;
using UnityEngine;

namespace Cameras {
    public enum CAMERA_MODE {
        PLAYER_VIEW,
        TOP_DOWN_VIEW
    }
    
    public class MainCamera : MonoBehaviour {
        [SerializeField] private CinemachineFreeLook bananaManCamera;
        [SerializeField] private CinemachineVirtualCamera topDownCamera;

        [SerializeField] private UIInventoriesActions uiInventoriesActions;
        
        private CinemachineCameraOffset _bananaManCameraOffset;

        public CAMERA_MODE cameraMode;
        
        private void Start() {
            _bananaManCameraOffset = bananaManCamera.GetComponentInChildren<CinemachineCameraOffset>();
            cameraMode = CAMERA_MODE.PLAYER_VIEW;
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

        public void AddToFOV(float acceleration) {
            //_cinemachineFollowZoom.m_Width = acceleration;
        }

        public void Switch_To_Camera_View(CAMERA_MODE cameraMode) {
            switch (cameraMode) {
                case CAMERA_MODE.PLAYER_VIEW:
                    bananaManCamera.m_Priority = 20;
                    topDownCamera.m_Priority = 5;
                    this.cameraMode = cameraMode;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.MOVE_ONE_AXIS_CAMERA].alpha = 0;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.ROTATE_ONE_AXIS_CAMERA].alpha = 0;
                    break;
                case CAMERA_MODE.TOP_DOWN_VIEW:
                    bananaManCamera.m_Priority = 5;
                    topDownCamera.m_Priority = 20;
                    uiInventoriesActions.activatedOneAxisRotationCamera = ObjectsReference.Instance.topDownCamera;
                    uiInventoriesActions.activatedOneAxisRotationCamera.ResetPosition();
                    this.cameraMode = cameraMode;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.MOVE_ONE_AXIS_CAMERA].alpha = 1;
                    ObjectsReference.Instance.uiManager.canvasGroupsByUICanvasType[UICanvasGroupType.ROTATE_ONE_AXIS_CAMERA].alpha = 1;
                    break;
            }
        }
    }
}
