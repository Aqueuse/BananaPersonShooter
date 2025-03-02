using UnityEngine;

namespace Cameras {
    public class CameraGestionDragRotate : MonoBehaviour {
        [SerializeField] private Transform cameraGestionYaxisTransform;
        
        public float cameraRotationSpeed;
        public float cameraMoveSpeed;
        public float cameraYposition;
        
        private Transform _transform;
        public Vector3 cameraMovement;

        private Vector3 cameraPosition;

        private Collider cameraBounds;
        
        private void OnEnable() {
            _transform = transform;
            ResetPosition();
        }
        
        public void Rotate(Vector2 rotation) {
            _transform.Rotate(0, rotation.x*cameraRotationSpeed, 0);
            cameraGestionYaxisTransform.Rotate(-rotation.y*cameraRotationSpeed, 0, 0);
        } 
        
        public void Move(float Xmovement, float Zmovement) {
            cameraMovement.x = Xmovement * cameraMoveSpeed;
            cameraMovement.y = 0;
            cameraMovement.z = Zmovement * cameraMoveSpeed;
            _transform.Translate(cameraMovement, _transform);
        }
        
        public void CancelMove() {
            cameraMovement = Vector3.zero;
        }
        
        public void ResetPosition() {
            var cameraTransform = ObjectsReference.Instance.bananaMan.transform.position;
            cameraTransform.y += cameraYposition;
            transform.position = cameraTransform;
        }
    }
}
