using UnityEngine;

namespace Cameras {
    public class CameraGestionRelativeMove : MonoBehaviour {
        public float moveSpeed = 30;

        private Transform _transform;
        private Vector3 cameraForward;

        public Vector3 movement;
        
        private Vector3 relativeMovement = Vector3.zero;
        
        private void OnEnable() {
            _transform = transform;
        }

        private void Update() {
            relativeMovement = (movement.y * ObjectsReference.Instance.mainCamera.transform.forward + movement.x * ObjectsReference.Instance.mainCamera.transform.right).normalized;

            _transform.position += relativeMovement * (moveSpeed * Time.deltaTime);
        }
        
        public void MoveWithKeyboard(Vector2 movement) {
            this.movement = movement;
        }
        
        public void CancelMove() {
            movement = Vector3.zero;
        }
    }
}
