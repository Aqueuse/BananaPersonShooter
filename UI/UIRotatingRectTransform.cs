using UnityEngine;

namespace UI {
    public class UIRotatingRectTransform : MonoBehaviour {
        [SerializeField] private RectTransform _loadingImageRectTransform;
        [SerializeField] private float speed;

        private Vector3 rotationEuler = Vector3.forward;
        private Quaternion rotation;

        private void Update() {
            rotationEuler.z -= Time.deltaTime * speed;
            if (rotationEuler.z < -360) rotationEuler.z = 0;

            rotation = Quaternion.Euler(rotationEuler);

            _loadingImageRectTransform.rotation = rotation;
        }
    }
}