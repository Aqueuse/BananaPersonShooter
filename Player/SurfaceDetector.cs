using UnityEngine;

namespace Player {
    public class SurfaceDetector : MonoSingleton<SurfaceDetector> {
        private int _layerMask;
        private Vector3 bananaManPosition;

        private void Start() {
            _layerMask = 1 << 9;
        }

        void Update() {
            // if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 200, _layerMask)) {
            //     var transformPosition = transform.position;
            //     transformPosition.y = raycastHit.point.y;
            //     BananaMan.Instance.transform.position = transformPosition;
            // }

            if (Physics.CheckSphere(transform.position, 0.25f, _layerMask)) {
                BananaMan.Instance.isInAir = false;
                BananaMan.Instance.tpsPlayerAnimator.IsInAir(false);
                BananaMan.Instance.tpsPlayerAnimator.IsGrounded(true);
            }

            else {
                BananaMan.Instance.isInAir = true;
                BananaMan.Instance.tpsPlayerAnimator.IsInAir(true);
                BananaMan.Instance.tpsPlayerAnimator.IsGrounded(false);
                
            }
        }
    }
}