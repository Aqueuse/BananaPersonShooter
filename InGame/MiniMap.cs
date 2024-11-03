using UnityEngine;

namespace InGame {
    public class MiniMap : MonoBehaviour {
        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private Transform miniMapCameraTransform;
        
        public void Move(Vector3 movement) {
            // keep it on a max distance from the center
            miniMapCameraTransform.position += movement;
            
            if (Vector3.Distance(ObjectsReference.Instance.commandRoomControlPanelsManager.transform.position,
                    miniMapCameraTransform.position) > 5000) {
                miniMapCameraTransform.position -= movement;
            }
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
