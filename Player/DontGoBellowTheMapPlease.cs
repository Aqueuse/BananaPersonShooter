using UnityEngine;

namespace Player {
    public class DontGoBellowTheMapPlease : MonoBehaviour {
        [SerializeField] private LayerMask walkableSurfaces;
        [SerializeField] private Transform groundCheck;
        private Vector3 _up;

        private Rigidbody _bananaManRigidBody;
        private Transform _bananaManTransform;

        public float surfaceY;

        private void Start() {
            _up = new Vector3(0, 1.1f, 0);

            _bananaManRigidBody = ObjectsReference.Instance.bananaMan.GetComponent<Rigidbody>();
            _bananaManTransform = ObjectsReference.Instance.bananaMan.transform;
        }

        private void FixedUpdate() {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit raycastHit, 100, walkableSurfaces)) {
                surfaceY = raycastHit.point.y;
            }
            
            if (groundCheck.position.y < surfaceY) {
                _bananaManRigidBody.isKinematic = true;
                _bananaManTransform.position = surfaceY*_up;
                _bananaManRigidBody.isKinematic = false;
            }
        }
    }
}
