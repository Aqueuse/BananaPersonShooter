using UnityEngine;

namespace Player {
    public class DontGoBellowTheMapPlease : MonoBehaviour {
        [SerializeField] private LayerMask walkableSurfaces;
        [SerializeField] private Transform groundCheck;
        private Vector3 up;

        public float surfaceY;

        private void Start() {
            up = new Vector3(0, 1, 0);
        }

        private void Update() {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit, 100, walkableSurfaces)) {
                surfaceY = raycastHit.point.y;
            }
            
            if (groundCheck.position.y < surfaceY) {
                ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = true;
                ObjectsReference.Instance.bananaMan.transform.position = surfaceY*up;
                ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }
}
