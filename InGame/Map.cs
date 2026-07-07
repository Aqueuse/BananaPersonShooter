using UnityEngine;

namespace InGame {
    public class Map : MonoBehaviour {
        [SerializeField] private Camera MapCamera;
        [SerializeField] private Camera MapUICamera;
        
        public void Drag(Vector3 dragMove) {
            MapCamera.transform.position += dragMove;
        }

        public void Zoom() {
            if (MapCamera.orthographicSize <= 10000) {
                MapCamera.orthographicSize += 20;
                MapUICamera.orthographicSize += 20;
            }
        }

        public void Dezoom() {
            if (MapCamera.orthographicSize > 20) {
                MapCamera.orthographicSize -= 20;
                MapUICamera.orthographicSize -= 20;
            }
        }
    }
}
