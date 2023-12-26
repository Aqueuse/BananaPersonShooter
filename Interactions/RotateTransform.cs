using UnityEngine;

namespace Interactions {
    public class RotateTransform : MonoBehaviour {
        private Transform _elementToRotateTransform;
        private const float _speed = 200;

        private Vector3 rotationEuler = Vector3.forward;
        private Quaternion rotation;

        private void Start() {
            _elementToRotateTransform = transform;
            Invoke(nameof(DestroyMe), 1.5f);
        }

        void Update() {
            rotationEuler.z -= Time.deltaTime * _speed;
            if (rotationEuler.z < -360) rotationEuler.z = 0;
            
            rotation = Quaternion.Euler(rotationEuler);

            _elementToRotateTransform.localRotation = rotation;
        }

        private void DestroyMe() {
            Destroy(this);
        }
    }
}
