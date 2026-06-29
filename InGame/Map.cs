using UnityEngine;

namespace InGame {
    public class Map : MonoBehaviour {
        [SerializeField] private Camera MapCamera;
        
        public void Drag(Vector3 dragMove) {
            MapCamera.transform.position += dragMove;
        }

        public void Zoom() {
            if (MapCamera.orthographicSize <= 10000)
                MapCamera.orthographicSize += 100;
        }

        public void Dezoom() { 
            if (MapCamera.orthographicSize >= 400)
                MapCamera.orthographicSize -= 100;
        }
    }
}
