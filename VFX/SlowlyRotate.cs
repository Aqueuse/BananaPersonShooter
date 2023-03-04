using UnityEngine;

namespace VFX {
    public class SlowlyRotate : MonoBehaviour {
        private float speed;
        public bool isRotating;

        private void Start() {
            speed = 30f;
            isRotating = true;
        }

        private void Update() {
            if (isRotating) {
                transform.Rotate(0, Time.deltaTime * speed, 0, Space.World);
            }
        }
    }
}
