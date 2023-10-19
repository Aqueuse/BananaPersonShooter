using UnityEngine;

namespace Cameras {
    public class CameraOneAxisRotation : MonoBehaviour {
        [SerializeField] private float rotationSpeed = 5f;
        public float cameraSpeed = 8f;

        [SerializeField] private Transform resetTransform;

        public float cameraY;

        private Transform _transform;
        private Vector3 cameraMovement;

        private void Start() {
            _transform = transform;
        }

        public void Move(float Xmovement, float Zmovement) {
            cameraMovement.x = Xmovement;
            cameraMovement.y = cameraY;
            cameraMovement.z = Zmovement;

            transform.Translate(cameraMovement * (cameraSpeed * Time.deltaTime));
        }

        public void Rotate(float rotationX) {
            _transform.Rotate(Vector3.up, rotationX*rotationSpeed * Time.deltaTime);
        }

        public void ResetPosition() {
            transform.position = resetTransform.position;
        }
    }
}
