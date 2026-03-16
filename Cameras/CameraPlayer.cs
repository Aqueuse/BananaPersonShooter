using UnityEngine;

namespace Cameras {
    public class CameraPlayer : MonoBehaviour {
        [SerializeField] private Camera bananaManCamera;
        [SerializeField] private Transform lookAtTarget;
        [SerializeField] private Transform Xrotation;
        [SerializeField] private Transform Yrotation;

        public float yCameraLimitUp = -0.17f;
        public float yCameraLimitbottom = 0.09f;
        
        private bool canRotate;
        
        private void Update() {
            if (!canRotate) return;

            transform.position = lookAtTarget.position;
            
            Xrotation.rotation *= Quaternion.Euler(new Vector3(
                0, 
                ObjectsReference.Instance.gameActions.look.y * ObjectsReference.Instance.gameSettings.horizontalLookSensibility, 
                0)
            );

            Yrotation.rotation *= Quaternion.Euler(new Vector3(
                ObjectsReference.Instance.gameActions.look.x * ObjectsReference.Instance.gameSettings.verticalLookSensibility,
                0,
                0));

            if (Yrotation.localRotation.x > yCameraLimitUp || Yrotation.localRotation.x < yCameraLimitbottom) {
                Yrotation.rotation *= Quaternion.Euler(new Vector3(
                    -ObjectsReference.Instance.gameActions.look.x * ObjectsReference.Instance.gameSettings.verticalLookSensibility,
                    0,
                    0));
            }
        }
        
        public void Set0Sensibility() {
            canRotate = false;
        }

        public void SetNormalSensibility() {
            canRotate = true;
        }
        
        public void ZoomCamera() {
            bananaManCamera.transform.position += Vector3.forward * 0.1f;
        }
        
        public void DezoomCamera() {
            bananaManCamera.transform.position -= Vector3.forward * 0.1f;
        }

        public void ActivateCamera() {
            bananaManCamera.enabled = true;
        }
        
        public void DeactivateCamera() {
            bananaManCamera.enabled = false;
        }
    }
}
