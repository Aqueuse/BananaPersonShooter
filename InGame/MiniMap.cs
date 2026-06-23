using UnityEngine;

namespace InGame {
    public class MiniMap : MonoBehaviour {
        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private Quaternion miniMapCameraRotation;

        private Vector3 cameraPosition;

        private void Update() {
            transform.rotation = miniMapCameraRotation;

            cameraPosition = ObjectsReference.Instance.bananaMan.transform.position;
            cameraPosition.y = 2720;

            transform.position = cameraPosition;
        }

        public void Zoom() {
            if (miniMapCamera.orthographicSize <= 10000)
                miniMapCamera.orthographicSize += 100;
        }

        public void Dezoom() { 
            if (miniMapCamera.orthographicSize >= 400)
                miniMapCamera.orthographicSize -= 100;
        }
    }
}
