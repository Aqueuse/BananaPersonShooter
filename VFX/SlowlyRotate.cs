using UnityEngine;

namespace VFX {
    public class SlowlyRotate : MonoBehaviour {
        private float _speed;
        public bool isRotating;

        private void Start() {
            _speed = 30f;
            isRotating = true;
        }

        private void Update() {
            if (isRotating) {
                transform.Rotate(0, Time.deltaTime * _speed, 0, Space.World);
            }
        }
    }
}
