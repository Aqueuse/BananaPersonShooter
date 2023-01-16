using UnityEngine;

namespace Player {
    public class RagDoll : MonoBehaviour {
        [Header("Root Skeleton")] public Transform root;
        
        private Rigidbody[] _rigidbodys;
        private bool _isRagDoll;
        
        private void Start() {
            _rigidbodys = root.GetComponentsInChildren<Rigidbody>();
        }
        
        public bool IsRagDoll() {
            return _isRagDoll;
        }

        public void SetRagDoll(bool active) {
            foreach (Rigidbody rgb in _rigidbodys) {
                rgb.detectCollisions = active;
                rgb.interpolation = active ? RigidbodyInterpolation.Interpolate : RigidbodyInterpolation.None;
                rgb.collisionDetectionMode = active ? CollisionDetectionMode.ContinuousSpeculative : CollisionDetectionMode.Discrete;
                rgb.isKinematic = !active;
            }
            
            _isRagDoll = active;
        }
    }
}